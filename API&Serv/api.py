from sanic import Sanic, response
from sqlalchemy import create_engine, or_
from sqlalchemy.orm import sessionmaker
from tables import User, Game, GameDTO, Friend, Base
import jwt as pyjwt
import hashlib 

JWT_SECRET = "OH@-WIIWtp1wKlL3[WVo'l-K0Vpxf(M#88S?JyDeFBM=t/iGA.Ar)_g?P}T.C'*;wYhcolUxF!A0y-]r+-7I6vC:Z!S}@bKii.QI"
WSS_TOKEN = "pdeylwASfuGlCdsSTWFFhGaLE"

app = Sanic(__name__)
engine = create_engine('mysql+mysqlconnector://root:@localhost/blockade')
Session = sessionmaker(bind=engine)
session = Session()

async def verify_user(request):
    whitelisted_routes = [
        '/users/login',
        '/users/create'
    ]
    
    for route in whitelisted_routes:
        if route in str(request.url):
            return None
    
    data = request.args
    if 'jwt' not in data :
        return response.json({'message': "JWT manquant"}, status=998)
    
    if data.get('jwt') == WSS_TOKEN:
        return None

    try:
        pyjwt.decode(data.get('jwt'), JWT_SECRET, algorithms=["HS256"])
        return None
    except pyjwt.DecodeError:
        return response.json({'message': "Le JWT n'est pas valide"}, status=999)

app.register_middleware(verify_user)

#---Users---

async def get_data_from_jwt(jwt):
    decoded = pyjwt.decode(jwt, JWT_SECRET, algorithms=["HS256"])
    return decoded

async def get_user_by_id(id):
    user = session.query(User).filter(User.userId == id).first()
    session.close()
    return user.to_dict()

async def check_username(user_name):
    user = session.query(User).filter(User.pseudo == user_name).first()
    session.close()
    if user:
        return user.to_dict()
    else:
        return None

async def create_user(name, mdp):
    userExist = await check_username(name)
    if userExist is None:
        try:
            session.add(User(pseudo=name, password=mdp))
            session.commit()
            session.close()
            return response.json({'message': 'User created successfully'}, status=200)
        except Exception as e:
            return response.json({'message': "Une erreur est survenu", 'error' : str(e)}, status=400) 
    else:
        return response.json({'message': "Pseudo déjà utilisé"}, status=400) 

async def login_user(name, mdp):
    userExist = await check_username(name)
    if userExist is not None:
        try:
            user = session.query(User).filter(User.pseudo == name, User.password == mdp).first()
            print(user)
            session.close()
            if user is not None:
                return True
            else:
                return response.json({'message': "Mauvais identifiants"}, status=400) 
        except Exception as e:
            return response.json({'message': "Une erreur est survenu", 'error' : str(e)}, status=400) 
    else:
        return response.json({'message': "Utilisateur inexistant"}, status=400)

async def add_friend(name, friendName):
    userExist = await check_username(name)
    friendExist = await check_username(friendName)
    if userExist is None or friendExist is None:
        return response.json({'message': 'Utilisateur inexistant'}, status=400)
    else:
        try:
            session.add(Friend(user1=userExist['id'], user2=friendExist['id'], isValid=False))
            session.commit()
            session.close()
            return response.json({'message': "Demande d'amis envoyé"}, status=200)
        except Exception as e:
            return response.json({'message': "Une erreur est survenu", 'error' : str(e)}, status=400)

async def accept_friend(id, friendId):
    userExist = await get_user_by_id(id)
    friendExist = await get_user_by_id(friendId)
    if userExist is None or friendExist is None:
        return response.json({'message': 'Utilisateur inexistant'}, status=400)
    else:
        try:
            rows_updated = session.query(Friend).filter(Friend.user1 == friendExist['id'], Friend.user2 == userExist['id']).update({Friend.isValid: True}, synchronize_session=False)
            session.commit()
            session.close()
            if rows_updated > 0:
                return response.json({'message': "Demande d'amis accepté"}, status=200)
            else:
                return response.json({'message': "No friend request found"}, status=400)
        except Exception as e:
            return response.json({'message': "Une erreur est survenu", 'error' : str(e)}, status=400)

async def get_friends(userId):
    friends = session.query(Friend).filter(or_(Friend.user1 == userId, Friend.user2 == userId), Friend.isValid == True).all()
    friend_list = []
    for friend in friends:
        friend_data = await get_user_by_id(friend.user2)
        friend_list.append(friend_data)
    return response.json({'friends': friend_list}, status=200)

async def get_pending_friend_requests(userId):
    friend_requests = session.query(Friend).filter(Friend.user2 == userId, Friend.isValid == False).all()
    friend_request_list = []
    for friend_request in friend_requests:
        friend_request_data = await get_user_by_id(friend_request.user1)
        friend_request_list.append(friend_request_data)
    return response.json({'friend_requests': friend_request_list}, status=200)

@app.route('/users/create', methods=['POST'])
async def create_user_handler(request):
    data = request.args
    name = data.get('name')
    password = data.get('password')
    if not name or not password:
        return response.json({'message': 'Missing required parameter'}, status=400)
    passHash = hashlib.sha256(password.encode('utf-8')).hexdigest()
    return await create_user(name, passHash)

@app.route('/users/login', methods=['GET'])
async def login_user_handler(request):
    data = request.args
    name = data.get('name')
    password = data.get('password')
    if not name or not password:
        return response.json({'message': 'Missing required parameter'}, status=400)
    passHash = hashlib.sha256(str(password).encode('utf-8')).hexdigest()
    loginResp = await login_user(name, passHash)
    if loginResp != True:
        return loginResp
    else:
        userforname = await check_username(name)
        encodedJWT = pyjwt.encode({'userid': userforname["id"], 'pseudo' : userforname["pseudo"]}, JWT_SECRET, algorithm="HS256")
        return response.json({'message': 'Login successful', 'jwt': encodedJWT, 'userId' : str(userforname["id"])}, status=200)

@app.route('/friends/accept', methods=['POST'])
async def accept_friend_request(request):
    data = request.args
    jwtData = await get_data_from_jwt(data.get('jwt'))
    id = jwtData['userid']
    friendId = data.get('friendId')
    if not friendId:
        return response.json({'message': 'Missing required parameter'}, status=400)
    return await accept_friend(id, friendId)

@app.route('/friends/add', methods=['POST'])
async def add_friend_handler(request):
    data = request.args
    jwtData = await get_data_from_jwt(data.get('jwt'))
    name = jwtData['pseudo']
    friendName = data.get('friendName')
    if not name or not friendName:
        return response.json({'message': 'Missing required parameter'}, status=400)
    return await add_friend(name, friendName)   

@app.route('/friends/requests', methods=['GET'])
async def get_friend_requests_handler(request):
    data = request.args
    jwtData = await get_data_from_jwt(data.get('jwt'))
    userId = jwtData['userid']
    return await get_pending_friend_requests(userId)

@app.route('/friends', methods=['GET'])
async def get_friends_handler(request):
    data = request.args
    jwtData = await get_data_from_jwt(data.get('jwt'))
    userId = jwtData['userid']
    return await get_friends(userId)

@app.route('/game/create', methods=['POST'])
async def create_game_handler(request):
    _gameId = 123456
    game = session.query(Game).filter(Game.gameId == _gameId).first()
    while game is not None:
        _gameId += 1
        game = session.query(Game).filter(Game.gameId == _gameId).first()
    data = request.json
    player1 = data.get('player1')
    player2 = data.get('player2')
    if not player1 or not player2:
        return response.json({'message': 'Missing required parameter'}, status=400)
    try:
        session.add(Game(gameId=_gameId, userId1=player1, userId2=player2, status=0))
        session.commit()
        session.close()
        return response.json({'message': 'Game created successfully', 'gameId': _gameId}, status=200)
    except Exception as e:
        return response.json({'message': "Une erreur est survenue", 'error' : str(e)}, status=400)

if __name__ == '__main__':
    Base.metadata.create_all(bind=engine)
    app.run(debug=True)