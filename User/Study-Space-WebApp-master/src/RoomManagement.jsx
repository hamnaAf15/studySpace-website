import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { FaSearch, FaPlus, FaUsers, FaLock, FaUnlock, FaClock, FaTrash } from 'react-icons/fa';
import axios from 'axios';
import './room.css';

const RoomManagement = () => {
  const [activeTab, setActiveTab] = useState('join');
  const [joinCode, setJoinCode] = useState('');
  const [roomName, setRoomName] = useState('');
  const [roomPrivacy, setRoomPrivacy] = useState(false);
  const [rooms, setRooms] = useState([]);

  const user = JSON.parse(localStorage.getItem('user'));

  const fetchRooms = async () => {
    try {
      const res = await axios.get('https://localhost:7183/api/StudyRoomsApi');
      const allRooms = res.data;
      const joinedRoomIds = JSON.parse(localStorage.getItem('joinedRooms')) || [];

      const visibleRooms = allRooms.filter(r =>
        !r.isPrivate || 
        r.createdBy === user?.userId || 
        joinedRoomIds.includes(r.roomId)
      );

      setRooms(visibleRooms);
    } catch (err) {
      console.error('Error loading rooms:', err);
    }
  };

  useEffect(() => {
    fetchRooms();
  }, []);

  const handleJoinRoom = async (e) => {
    e.preventDefault();

    try {
      const res = await axios.get('https://localhost:7183/api/StudyRoomsApi');
      const allRooms = res.data;

      const matchedRoom = allRooms.find(r =>
        r.isPrivate && r.accessCode === joinCode
      );

      if (matchedRoom) {
        const joinedRoomIds = JSON.parse(localStorage.getItem('joinedRooms')) || [];
        if (!joinedRoomIds.includes(matchedRoom.roomId)) {
          joinedRoomIds.push(matchedRoom.roomId);
          localStorage.setItem('joinedRooms', JSON.stringify(joinedRoomIds));
        }

        setRooms(prev => {
          const exists = prev.find(r => r.roomId === matchedRoom.roomId);
          return exists ? prev : [...prev, matchedRoom];
        });

        alert(`Access granted to private room "${matchedRoom.roomName}"`);
        setJoinCode('');
      } else {
        alert('Invalid or unauthorized room code.');
      }
    } catch (err) {
      console.error('Error checking room code:', err);
    }
  };

  const handleCreateRoom = async (e) => {
    e.preventDefault();

    if (!roomName.trim()) return;

    const newRoom = {
      roomName,
      isPrivate: roomPrivacy,
      accessCode: roomPrivacy ? Math.random().toString(36).substring(2, 8).toUpperCase() : null,
      createdBy: user?.userId,
      createdAt: new Date().toISOString()
    };

    try {
      const res = await axios.post('https://localhost:7183/api/StudyRoomsApi', newRoom);
      setRooms(prev => [...prev, res.data]);

      alert(`Room "${res.data.roomName}" created successfully!\nAccess Code: ${res.data.accessCode || 'Public - No Code Required'}`);
      setRoomName('');
      setRoomPrivacy(false);
      setActiveTab('join');
    } catch (err) {
      console.error('Room creation failed:', err);
    }
  };

  const handleDeleteRoom = async (roomId) => {
    const confirmDelete = window.confirm('Are you sure you want to delete this room?');
    if (!confirmDelete) return;

    try {
      await axios.delete(`https://localhost:7183/api/StudyRoomsApi/${roomId}`);
      setRooms(prev => prev.filter(r => r.roomId !== roomId));
    } catch (err) {
      console.error('Error deleting room:', err);
      alert('Failed to delete room. You may not have permission.');
    }
  };

  return (
    <div className="container py-5">
      <div className="text-center mb-5">
        <h2 className="fw-bold">Study Rooms</h2>
        <p className="text-muted">Collaborate with peers in dedicated study spaces</p>
      </div>

      <div className="d-flex justify-content-center gap-3 mb-4">
        <button
          className={`btn ${activeTab === 'join' ? 'btn-primary' : 'btn-outline-primary'}`}
          onClick={() => setActiveTab('join')}
        >
          <FaSearch className="me-2" />
          Find Rooms
        </button>
        <button
          className={`btn ${activeTab === 'create' ? 'btn-primary' : 'btn-outline-primary'}`}
          onClick={() => setActiveTab('create')}
        >
          <FaPlus className="me-2" />
          Create Room
        </button>
      </div>

      {activeTab === 'join' ? (
        <>
          <div className="mb-4 d-flex justify-content-center">
            <input
              type="text"
              className="form-control form-control-lg w-75"
              placeholder="Search rooms..."
            />
          </div>

          <div className="room-list mb-5">
            {rooms.map((room) => (
              <div
                key={room.roomId}
                className="room-card p-3 mb-3 border rounded d-flex justify-content-between align-items-center shadow-sm"
              >
                <div>
                  <h5 className="mb-2">{room.roomName}</h5>
                  <div className="d-flex gap-3 text-muted small">
                    <span>
                      <FaUsers className="me-1" />
                      Unknown members
                    </span>
                    <span>
                      {room.isPrivate ? (
                        <span className="badge bg-danger">
                          <FaLock className="me-1" /> Private
                        </span>
                      ) : (
                        <span className="badge bg-success">
                          <FaUnlock className="me-1" /> Public
                        </span>
                      )}
                    </span>
                    <span>
                      <FaClock className="me-1" />
                      {room.createdAt?.substring(0, 10)}
                    </span>
                  </div>
                </div>

                <div className="d-flex gap-2">
                  <Link to={`/study-room/${room.roomId}`} className="btn btn-outline-primary btn-sm">
                    Join
                  </Link>
                  {room.createdBy === user?.userId && (
                    <button
                      className="btn btn-outline-danger btn-sm"
                      onClick={() => handleDeleteRoom(room.roomId)}
                    >
                      <FaTrash />
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>

          <div className="w-75 mx-auto">
            <h5 className="text-primary mb-3">Join Private Room by Code</h5>
            <form onSubmit={handleJoinRoom} className="d-flex gap-2">
              <input
                type="text"
                className="form-control"
                placeholder="Enter room code"
                value={joinCode}
                onChange={(e) => setJoinCode(e.target.value)}
              />
              <button type="submit" className="btn btn-success">
                Join
              </button>
            </form>
          </div>
        </>
      ) : (
        <form onSubmit={handleCreateRoom} className="w-75 mx-auto">
          <div className="mb-4 form-floating">
            <input
              type="text"
              id="roomName"
              className="form-control"
              placeholder="Room Name"
              value={roomName}
              onChange={(e) => setRoomName(e.target.value)}
              required
            />
            <label htmlFor="roomName">Room Name</label>
          </div>

          <fieldset className="mb-4">
            <legend className="fw-semibold">Room Type</legend>
            <div className="d-flex gap-3">
              <label className="d-flex align-items-center gap-2 w-50">
                <input
                  type="radio"
                  name="privacy"
                  checked={!roomPrivacy}
                  onChange={() => setRoomPrivacy(false)}
                />
                <div className="border rounded p-2 w-100">
                  <FaUnlock className="text-success me-2" />
                  <strong>Public</strong>
                  <p className="mb-0 small text-muted">Anyone can join</p>
                </div>
              </label>

              <label className="d-flex align-items-center gap-2 w-50">
                <input
                  type="radio"
                  name="privacy"
                  checked={roomPrivacy}
                  onChange={() => setRoomPrivacy(true)}
                />
                <div className="border rounded p-2 w-100">
                  <FaLock className="text-danger me-2" />
                  <strong>Private</strong>
                  <p className="mb-0 small text-muted">Invite-only with code</p>
                </div>
              </label>
            </div>
          </fieldset>

          <button type="submit" className="btn btn-primary w-100 py-2 fs-5">
            Create Room
          </button>
        </form>
      )}
    </div>
  );
};

export default RoomManagement;
