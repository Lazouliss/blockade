const jwt = require('jsonwebtoken');
const WebSocket = require('ws');

const version = '1.0.4';

const wssPort = 27015;
const secretKey = "OH@-WIIWtp1wKlL3[WVo'l-K0Vpxf(M#88S?JyDeFBM=t/iGA.Ar)_g?P}T.C'*;wYhcolUxF!A0y-]r+-7I6vC:Z!S}@bKii.QI";
const wssToken = "pdeylwASfuGlCdsSTWFFhGaLE";

function isValidJWT(token){
	return new Promise(resolve => {
        jwt.verify(token, secretKey, (err, decoded) => {
            resolve(true);
        });
    });
}

async function ParseAndValidate(text){
	try{
		let json = JSON.parse(text)
		if(!await isValidJWT(json.token))
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
				console.log({ error: 'Error while parsing JSON!' });
				break;
		}
	}
}

function userExists(id){
	// TODO API: retourne true pour les tests, il faut faire un check dans la base de données pour vérifier que l'id de l'utilisateur existe bien dans la base de données
	return true
}

let connCnt = 2700;
let lobbyCnt = 1;
let gameCnt = 1;
let lstConn = [];
let lstLobby = [];
let lstGame = [];

function isUserAvailable(userId){
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

function generateLobbyCode(length) {
	const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
	let result = '';
	for (let i = 0; i < length; i++) 
		result += characters.charAt(Math.floor(Math.random() * characters.length));
	return '#' + result;
}


const wss = new WebSocket.Server({ port: wssPort });

wss.on('connection', conn => {
	console.log('Logged connection ' + connCnt);
	conn.connId = connCnt++;
	lstConn.push(conn);
    conn.on('message', async msg => {
		console.log(msg.toString());
		let json = await ParseAndValidate(msg.toString());
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
				case 'lobby-host':
					if(!isUserAvailable(json.userId))
						return;
					
					let pwd = generateLobbyCode(6);
					while(lstLobby.find(_lobby => _lobby.pwd == pwd))
						pwd = generateLobbyCode(6);
					
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
					conn.send(JSON.stringify(res))
					break;
					

				case 'lobby-close':
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId)
					if(!lobby)
						return;
					
					var guestConn = lstConn.find(_conn => _conn.userId == lobby.guest)
					if(guestConn){
						var req = { requestType: 'lobby-lost-host' }
						guestConn.send(JSON.stringify(req))
					}
					
					console.log(`User ${json.userId} has closed his lobby!`);
					lstLobby = lstLobby.filter(_lobby => _lobby.lobbyId != lobby.lobbyId)
					break;


				case 'lobby-join':
					if(!isUserAvailable(json.userId))
						return;
					
					var lobby = lstLobby.find(_lobby => _lobby.pwd == json.data)
					if(!lobby){
						console.log(`No lobby found with pwd: ${json.data} (userId: ${json.userId})`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'No lobby found!'
						}
						conn.send(JSON.stringify(res))
						return;
					}
					if(lobby.guest){
						console.log(`Lobby ${lobby.lobbyId} has already a guest! (userId: ${json.userId})`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'Already a guest in this lobby!'
						}
						conn.send(JSON.stringify(res))
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
					hostConn.send(JSON.stringify(req))

					var res = {
						result: 200,
						responseType: json.requestType,
						data: json.data,
						msg: 'Succesfully joined lobby'
					}
					conn.send(JSON.stringify(res))
					break;


				case 'lobby-leave':
					var lobby = lstLobby.find(_lobby => _lobby.guest == conn.userId)
					if(!lobby)
						return;
					
					// TODO IA: Gérer la connexion de l'IA à la partie lorsqu'un utilisateur se déconnecte de la partie
					var hostConn = lstConn.find(_conn => _conn.userId == lobby.host)
					var req = { requestType: 'lobby-lost-guest' }
					hostConn.send(JSON.stringify(req))
					
					console.log(`User ${json.userId} has left a lobby!`);
					lobby.guest = null;
					break;
				

				case 'lobby-send-msg':
					var lobby = lstLobby.find(_lobby => [_lobby.host, _lobby.guest].includes(conn.userId))
					if(!lobby){
						console.log(`User ${conn.userId} tries to send lobby msg but he is not in any lobby!`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'You are not in any lobby or host!'
						}
						conn.send(JSON.stringify(res))
						return;
					}
					var otherPlayer = lobby.host == conn.userId ? lobby.guest : lobby.host
					if(otherPlayer){
						var otherConn = lstConn.find(_conn => _conn.userId == otherPlayer)
						var req = {
							requestType: json.requestType,
							data: json.data
						}
						otherConn.send(JSON.stringify(req))
						
						var res = {
							result: 200,
							responseType: json.requestType,
							data: json.data,
							msg: 'Successfully sent message'
						}
						conn.send(JSON.stringify(res))
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
						conn.send(JSON.stringify(res))
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
						conn.send(JSON.stringify(res));
						return;
					}
					var req = {
						requestType: json.requestType,
						data: [json.data[1]]
					}
					guestConn.send(JSON.stringify(req))
					break;


				case 'friend-request':
					if(userExists(json.data)){
						var otherConn = lstConn.find(_conn => _conn.connId == json.data)
						if(otherConn != null){
							var req = {
								requestType: json.requestType,
								data: null
							}
							otherConn.send(JSON.stringify(req))
						}

						//TODO API: faire l'appel API pour ajouter la demande d'ami dans la base de données
						
						var res = {
							result: 200,
							responseType: json.requestType,
							msg: `Successfully sent friend request to user ${json.data}`
						}
						conn.send(JSON.stringify(res))
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
						otherConn.send(JSON.stringify(req))
					}

					//TODO API: faire l'appel API pour ajouter les deux utilisateurs en ami
						
					var res = {
						result: 200,
						responseType: json.requestType,
						msg: `Successfully accepted friend request from user ${json.data}`
					}
					conn.send(JSON.stringify(res))
					break;

				//PARTIE

				case 'lobby-start-game':
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId)
					if(!lobby){
						console.log(`User ${conn.userId} tries to start a game but he is not in any lobby or is not host!`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'You are not in any lobby or host!'
						}
						conn.send(JSON.stringify(res))
						return;
					}
					if(!lobby.guest){
						console.log(`User ${conn.userId} tries to start a game but there is no guest!`);
						var res = {
							result: 401,
							responseType: json.requestType,
							msg: 'There is nobody else in your room!'
						}
						conn.send(JSON.stringify(res))
						return;
					}

					//TODO API: faire l'appel API créant la partie et renvoyant le gameId
					var gameId = 0;
					fetch('http://127.0.0.1:8000/game/create?jwt='+wssToken, {
						method: 'POST',
						headers: {
							'Content-Type': 'application/json'
						},
						body: JSON.stringify({
							player1: lobby.host,
							player2: lobby.guest
						})
					})
					.then(response => response.json())
					.then(data => {gameId = data.gameId
					var game = {
						gameId: gameId,
						player1: lobby.host,
						player2: lobby.guest
					}
					lstGame.push(game)
					//lstLobby = lstLobby.filter(_lobby => _lobby.lobbyId != lobby.lobbyId)
					
					var guestConn = lstConn.find(_conn => _conn.userId == lobby.guest)
					var req = {
						requestType: json.requestType,
						msg: 'Launching the game!',
						data: gameId
					}
					guestConn.send(JSON.stringify(req))

					var res = {
						result: 200,
						responseType: json.requestType,
						msg: 'Launching the game!',
						data: gameId
					}
					conn.send(JSON.stringify(res))})
					break;
				
				
				case 'action-movement':
					var lobby = lstLobby.find(_lobby => [_lobby.host, _lobby.guest].includes(conn.userId));
					var otherPlayer = lobby.host == conn.userId ? lobby.guest : lobby.host;
					var otherConn = lstConn.find(_conn => _conn.userId == otherPlayer);
					
					if(!lobby || !otherPlayer || !otherConn)
						return;

					var req = {
						requestType: json.requestType,
						data: json.data
					}
					otherConn.send(JSON.stringify(req))
					break;


				case 'action-end':
					var lobby = lstLobby.find(_lobby => _lobby.host == conn.userId);
					var otherPlayer = lobby.host == conn.userId ? lobby.guest : lobby.host;
					var otherConn = lstConn.find(_conn => _conn.userId == otherPlayer);

					if(!lobby || !otherPlayer || !otherConn)
						return;

					var req = {
						requestType: json.requestType,
						data: json.data
					}
					otherConn.send(JSON.stringify(req))
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

		//WEBSOCKET ERROR
		var CLOSE_GOING_AWAY = 1001;
		var CLOSE_ABNORMAL = 1006;

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
					guestConn.send(JSON.stringify(data))
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

console.log(`Server running at http://localhost:${wssPort}/ V ${version}`);

function PrintAllLobbies(){
    console.log("\n\n----> ALL lobbies");
    lstLobby.forEach(_lobby => {
        console.log("lobby.lobbyId: " + _lobby.lobbyId);
        console.log("lobby.host: " + _lobby.host);
        console.log("lobby.guest: " + _lobby.guest);
        console.log("---- ");
    })
    console.log("<----------- ");
}

setInterval(PrintAllLobbies, 20 * 1000);