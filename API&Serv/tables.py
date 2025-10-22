from sqlalchemy import Column, Integer, VARCHAR, TEXT, BOOLEAN
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

class User(Base):
    __tablename__ = 'users'
    userId  = Column(Integer, primary_key=True)
    pseudo = Column(VARCHAR(20))
    password = Column(TEXT)

    def to_dict(self):
        return {
            'id': self.userId,
            'pseudo': self.pseudo
        }
    
class Game(Base):
    __tablename__ = 'games'
    gameId  = Column(Integer, primary_key=True)
    status = Column(Integer)
    userId1 = Column(Integer)
    userId2 = Column(Integer)


    def to_dict(self):
        return {
            'gameId': self.gameId,
            'status': self.status,
            'userId1': self.userId1,
            'userId2' : self.userId2
        }

class GameDTO(Base):
    __tablename__ = 'gamedtos'
    dtoId = Column(Integer, primary_key=True)
    gameId  = Column(Integer)
    timestamp = Column(Integer)
    jsonDataDTO = Column(TEXT)
    nbTour = Column(Integer)


    def to_dict(self):
        return {
            'dtoId': self.dtoId,
            'gameId': self.gameId,
            'timestamp' : self.timestamp,
            'jsonDataDTO': self.jsonDataDTO,
            'nbTour' : self.nbTour
        }

class Friend(Base):
    __tablename__ = 'friends'
    id = Column(Integer, primary_key=True)
    user1  = Column(Integer)
    user2 = Column(Integer)
    isValid = Column(BOOLEAN)


    def to_dict(self):
        return {
            'id': self.id,
            'user1': self.user1,
            'user2' : self.user2,
            'isValid' : self.isValid
        }