const http = require('http');
const crypto = require('crypto');
const jwt = require('jsonwebtoken');
const WebSocketServer = require('websocket').server;

const version = '1.0.4';

//WEBSERVER

async function get(url){
	return new Promise(res => {
		http.get(url, response => {
			let data = '';
			response.on('data', chunk => data += chunk);
			response.on('end', () => res(data));
		});
	});
}

const server = http.createServer((req, res) => {
	res.setHeader('Access-Control-Allow-Origin', '*');
	res.writeHead(200, { 'Content-Type': 'application/json' });
	res.end(JSON.stringify({ message: 'Hello World http! V'+ version }));
});

const PORT = 27015;
server.listen(PORT, async() => {
	//get('http://api.ipify.org/').then(ip => console.log(`Server running at http://${ip}:${PORT}/ V ${version}`));
	console.log(`Server running at http://localhost:${PORT}/ V ${version}`);
});


const secretKey = crypto.randomBytes(32).toString('hex');

function validToken(token){
	return token == '0x0';

	jwt.verify(token, secretKey, (err, decoded) => {
		if (err) {
		  console.error('Failed to verify token:', err);
		  return;
		}
		
		console.log('Decoded JWT payload:', decoded);
	  });
}

//WEBSOCKET

//WEBSOCKET ERROR
const CLOSE_GOING_AWAY = 1001;
const CLOSE_ABNORMAL = 1006;

function parse_n_validate(text, res){
	try{
		let json = JSON.parse(text)
		if (!validToken(json.token))
			throw 'INV_TOKEN';
		return json;
	}
	catch(error){
		if(res){
			res.writeHead(404, { 'Content-Type': 'application/json' });
			if(error == 'INV_TOKEN')
				res.end(JSON.stringify({ error: 'Invalid token!' }));		
			else			
				res.end(JSON.stringify({ error: 'Couldnt parse the json!' }));		
		}
		return null;
	}
}

function userExists(id){
	// TODO API: retourne true pour les tests, il faut faire un check dans la base de données pour vérifier que l'id de l'utilisateur existe bien dans la base de données
	return true
}

function parse_n_validate_ws(text){
	try{
		let json = JSON.parse(text)
		if (!validToken(json.token))
			throw 'INV_TOKEN';
		if (!json.userId)
			throw 'NO_USERID';
		if (!json.requestType)
			throw 'NO_REQUEST_TYPE';
		return json;
	}
	catch(error){
		switch(error){
			case 'INV_TOKEN':
				console.log({ error: 'Invalid token!' });
				break;
			case 'NO_USERID':
				console.log({ error: 'No user id!' });
				break;
			case 'NO_REQUEST_TYPE':
				console.log({ error: 'No request type!' });
				break;
			default:
				console.log({ error: 'Error!' });
				break;
		}
	}
}

let connCnt = 2700;
let lobbyCnt = 1;
let gameCnt = 1;
let lstConn = [];
let lstLobby = [];
let lstGame = [];

function isUserFree(userId){
	let isHosting = lstLobby.find(lobby => lobby.host == userId)
	let isGuest = lstLobby.find(lobby => lobby.guest == userId)
	let isPlaying = lstGame.find(game => [game.player1, game.player1].includes(userId))
	if (isHosting)
		console.log(`User ${userId} is already hosting a lobby!`);
	if (isGuest)    
		console.log(`User ${userId} has already joined a lobby!`);
	if (isPlaying)  
		console.log(`User ${userId} is already playing a game!`);
	return !(isHosting || isGuest || isPlaying)
}

function generateCode(length) {
	const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
	let result = '';
	for (let i = 0; i < length; i++) {
		result += characters.charAt(Math.floor(Math.random() * characters.length));
	}
	return '#' + result;
}

const ws = new WebSocketServer({ httpServer: server, autoAcceptConnections: false });

ws.on('request', req => {
    const conn = req.accept(null, req.origin);
	console.log('Logged connection ' + connCnt);
	conn.connId = connCnt++;
	lstConn.push(conn);
    conn.on('message', msg => {
		let json = parse_n_validate_ws(msg.utf8Data);
		if(json){
			if(!conn.userId){
				if(lstConn.find(_conn => _conn.userId === json.userId)){
					conn.close(3000, `Connection with userid ${json.userId} already exists!`);
					return;
				}
				conn.userId = json.userId
				console.log(`\n----> Connection established with ${conn.remoteAddress} (connId: ${conn.connId}) (userId: ${conn.userId})`);
			}
			else if(conn.userId != json.userId){
				console.log(`Connection userid (${conn.userId}) differs from request userid (${json.userId})!!`)
				return;
			}
			switch(json.requestType){
				case 'lobby-host':	//Requête de l'hôte pour pouvoir héberger un lobby
					if(!isUserFree(json.userId))
						return;
					
					let pwd = generateCode(6);
					while(lstLobby.find(_lobby => _lobby.pwd == pwd))
						pwd = generateCode(6);
					
					var lobby = {
						lobbyId: lobbyCnt++,
						host: json.userId,
						pwd: pwd
					}
					lstLobby.push(lobby);
					console.log(`User ${json.userId} is hosting lobby ${lobby.lobbyId} with pwd: ${lobby.pwd}`);
					var res = {
						result: 200,
						responseType: json.requestType,
						data: pwd,
						msg: 'Succesfully created lobby'
					}
					conn.sendUTF(JSON.stringify(res))
					break;
					
				case 'lobby-close':	//Ordre de l'hôte pour fermer le lobby
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId)
					if(!lobby)
						return;
					
					var guestConn = lstConn.find(_conn => _conn.userId == lobby.guest)
					if(guestConn){
						var req = { requestType: 'lobby-lost-host' }
						guestConn.sendUTF(JSON.stringify(req))
					}
					
					console.log(`User ${json.userId} has closed his lobby!`);
					lstLobby = lstLobby.filter(_lobby => _lobby.lobbyId != lobby.lobbyId)
					break;

				case 'lobby-join':	//Requête d'un invité pour pouvoir rejoindre un lobby
					if(!isUserFree(json.userId))
						return;
					
					var lobby = lstLobby.find(_lobby => _lobby.pwd == json.data)
					if(!lobby){
						console.log(`No lobby found with pwd: ${json.data} (userId: ${json.userId})`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'No lobby found!'
						}
						conn.sendUTF(JSON.stringify(res))
						return;
					}
					if(lobby.guest){
						console.log(`Lobby ${lobby.lobbyId} has already a guest! (userId: ${json.userId})`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'Already a guest in this lobby!'
						}
						conn.sendUTF(JSON.stringify(res))
						lstConn = lstConn.filter(_conn => _conn != conn)
						return;
					}
					lobby.guest = json.userId
					console.log(`User ${json.userId} has joined lobby ${lobby.lobbyId}`);
					
					var hostConn = lstConn.find(_conn => _conn.userId == lobby.host);
					var req = {
						requestType: json.requestType,
						data: json.userId
					}
					hostConn.sendUTF(JSON.stringify(req))

					var res = {
						result: 200,
						responseType: json.requestType,
						data: json.data,
						msg: 'Succesfully joined lobby'
					}
					conn.sendUTF(JSON.stringify(res))
					break;

				case 'lobby-leave':	//Ordre d'un invité pour quitter le lobby
					var lobby = lstLobby.find(_lobby => _lobby.guest == conn.userId)
					if(!lobby)
						return;
					
					// TODO IA: Gérer la connexion de l'IA à la partie lorsqu'un utilisateur se déconnecte de la partie
					var hostConn = lstConn.find(_conn => _conn.userId == lobby.host)
					var req = { requestType: 'lobby-lost-guest' }
					hostConn.sendUTF(JSON.stringify(req))
					
					console.log(`User ${json.userId} has left a lobby!`);
					lobby.guest = null;
					break;
				
				case 'lobby-send-msg':	//Requete d'un user pour communiquer dans le chat dans un lobby
					var lobby = lstLobby.find(_lobby => [_lobby.host, _lobby.guest].includes(conn.userId))
					if(!lobby){
						console.log(`User ${conn.userId} tries to send lobby msg but he is not in any lobby!`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'You are not in any lobby or host!'
						}
						conn.sendUTF(JSON.stringify(res))
						return;
					}
					var otherPlayer = lobby.host == conn.userId ? lobby.guest : lobby.host
					if(otherPlayer){
						let otherConn = lstConn.find(_conn => _conn.userId == otherPlayer)
						var req = {
							requestType: json.requestType,
							data: json.data
						}
						otherConn.sendUTF(JSON.stringify(req))
						
						var res = {
							result: 200,
							responseType: json.requestType,
							data: json.data,
							msg: 'Successfully sent message'
						}
						conn.sendUTF(JSON.stringify(res))
					}
					else{
						console.log(`User ${conn.userId} tries to send lobby msg but he is alone!`);
					}
					break;
					
				case 'lobby-invite':
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId)
					if(!lobby || lobby.guest){
						var res = {
							result: 401,
							responseType: json.requestType
						}
						if(!lobby){
							console.log(`User ${conn.userId} tries to invite but is not hosting!`);
							res.msg = 'You are not hosting!'
						}
						else{
							console.log(`User ${conn.userId} tries to invite but there is already a guest!`);
							res.msg = 'Already a guest!'
						}
						conn.sendUTF(JSON.stringify(res))
						return;
					}
					var guestConn = lstConn.find(_conn => _conn.connId == json.data[0])
					if(!guestConn){
						var res = {
							result: 401,
							responseType: json.requestType,
							data: 'No user connected found with this id!'
						}
						console.log(`User ${conn.userId} tries to invite but nobody with this id!`);
						conn.sendUTF(JSON.stringify(res));
						return;
					}
					var req = {
						requestType: json.requestType,
						data: [json.data[1]]
					}
					guestConn.sendUTF(JSON.stringify(req))
					break;
				case 'lobby-start-game': //Requete d'un hôte pour pouvoir commencer la partie, pas encore complétée, par sûr que l'on fonctionne avec websocket !!!!
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId)
					if(!lobby){
						console.log(`User ${conn.userId} tries to start a game but he is not in any lobby or is not host!`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'You are not in any lobby or host!'
						}
						conn.sendUTF(JSON.stringify(res))
						return;
					}
					if(!lobby.guest){
						console.log(`User ${conn.userId} tries to start a game but there is no guest!`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'There is nobody else in your room!'
						}
						conn.sendUTF(JSON.stringify(res))
						return;
					}

					//TODO API: faire l'appel API créant la partie et renvoyant le gameId
					var game = {
						gameId: gameCnt++,
						player1: lobby.host,
						player2: lobby.guest,
						board: []
					}
					lstGame.push(game)
					lstLobby = lstLobby.filter(_lobby => _lobby.lobbyId != lobby.lobbyId)
					
					var guestConn = lstConn.find(_conn => _conn.userId == lobby.guest)
					var req = {
						requestType: json.requestType,
						msg: 'Launching the game!'
					}
					guestConn.sendUTF(JSON.stringify(req))

					var res = {
						result: 200,
						responseType: json.requestType,
						msg: 'Launching the game!'
					}
					conn.sendUTF(JSON.stringify(res))
					break;
				case 'friend-request':
					if(userExists(json.data)){
						let otherConn = lstConn.find(_conn => _conn.connId == json.data)
						if(otherConn != null){
							var req = {
								requestType: json.requestType,
								data: null
							}
							otherConn.sendUTF(JSON.stringify(req))
						}

						//TODO API: faire l'appel API pour ajouter la demande d'ami dans la base de données
						
						var res = {
							result: 200,
							responseType: json.requestType,
							msg: `Successfully sent friend request to user ${json.data}`
						}
						conn.sendUTF(JSON.stringify(res))
					}
					else{
						console.log(`User ${conn.connId} tries to send a friend request to a non existing user!`);
					}
					break;
				case 'friend-accept':
					var otherConn = lstConn.find(_conn => _conn.connId == json.data)
					if(otherConn != null){
						var req = {
							requestType: json.requestType,
							data: null
						}
						otherConn.sendUTF(JSON.stringify(req))
					}

					//TODO API: faire l'appel API pour ajouter les deux utilisateurs en ami
						
					var res = {
						result: 200,
						responseType: json.requestType,
						msg: `Successfully accepted friend request from user ${json.data}`
					}
					conn.sendUTF(JSON.stringify(res))
					break;
				case 'action-end':
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId);
					var otherPlayer = lobby.host == conn.userId ? lobby.guest : lobby.host;
					var otherConn = lstConn.find(_conn => _conn.userId == otherPlayer);
					var req = {
						requestType: json.requestType,
						data: json.data
					}
					otherConn.sendUTF(JSON.stringify(req))
					break;
				default:
					console.log(`Received unknown request type! ${json.requestType}`)
					break;
			}
		}
    });

    conn.on('close', (reasonCode, description) => {
        console.log(`\n----> Peer ${conn.remoteAddress} disconnected, (reasoncode: ${reasonCode}) (description: ${description}) (connId: ${conn.connId}) (userId: ${conn.userId})`);
		lstConn = lstConn.filter(_conn => _conn != conn)
		// TODO IA: Gérer la connexion de l'IA à la partie lorsqu'un utilisateur perd la connexion avec la socket
		if([CLOSE_GOING_AWAY, CLOSE_ABNORMAL].includes(reasonCode)){
			let lobbyHosted = lstLobby.find(_lobby => _lobby.host == conn.userId)
			let lobbyJoined = lstLobby.find(_lobby => _lobby.guest == conn.userId)
			if(lobbyHosted){
				if(lobbyHosted.guest){
					let guestConn = lstConn.find(_conn => _conn.userId == lobbyHosted.guest)
					var data = {
						requestType: 'lobby-lost-host',
						data: null
					}
					guestConn.sendUTF(JSON.stringify(data))
				}
				lstLobby = lstLobby.filter(_lobby => _lobby.host != conn.userId)
			}
			if(lobbyJoined){
				let hostConn = lstConn.find(_conn => _conn.userId == lobbyJoined.host)
				var data = {
					requestType: 'lobby-lost-guest',
					data: null
				}
				hostConn.send(JSON.stringify(data));
				lobbyJoined.guest = null;
			}
		}
    });
});

function PrintAllCon(){
	const desiredAttributes = ['connId', 'userId'];

	const filteredConnections = lstConn.map(connection => {
		const filteredConnection = {};
		desiredAttributes.forEach(attribute => {
			if (connection.hasOwnProperty(attribute)) {
				filteredConnection[attribute] = connection[attribute];
			}
		});
		return filteredConnection;
	});

	console.log('\n', filteredConnections, '\n');
}
//setInterval(PrintAllCon, 30000)