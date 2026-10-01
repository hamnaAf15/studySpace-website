import React, { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Sroom.css';
import { FaPaperclip, FaHandshake, FaBan } from 'react-icons/fa';

const StudyRoom = () => {
  const { id: roomId } = useParams();
  const navigate = useNavigate();

  const [roomName, setRoomName] = useState('');
  const [participants, setParticipants] = useState([]);
  const [messages, setMessages] = useState([]);
  const [message, setMessage] = useState('');
  const [todoList, setTodoList] = useState([]);
  const [newTodo, setNewTodo] = useState('');
  const [tool, setTool] = useState('pencil');
  const [color, setColor] = useState('#000000');

  const canvasRef = useRef(null);
  const isDrawing = useRef(false);

  const currentUser = JSON.parse(localStorage.getItem('user'));

  useEffect(() => {
    fetchRoomName();
    fetchParticipants();
    fetchMessages();
    fetchTodos();
    setupCanvas();
  }, [roomId]);

  const fetchRoomName = async () => {
    try {
      const res = await axios.get(`https://localhost:7183/api/StudyRoomsApi/${roomId}`);
      setRoomName(res.data.roomName || `Room ${roomId}`);
    } catch (err) {
      console.error('Error fetching room name', err);
    }
  };

  const setupCanvas = () => {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const ctx = canvas.getContext('2d');
    canvas.width = 1000;
    canvas.height = 600;
    ctx.fillStyle = 'white';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
  };

  const fetchParticipants = async () => {
    try {
      const res = await axios.get(`https://localhost:7183/api/RoomMembersApi?roomId=${roomId}`);
      setParticipants(res.data.map(p => ({
        id: p.userId,
        name: p.user?.fullName || 'User',
        online: true
      })));
    } catch (err) {
      console.error('Error fetching participants', err);
    }
  };

  const fetchMessages = async () => {
    try {
      const res = await axios.get(`https://localhost:7183/api/ChatMessagesApi?roomId=${roomId}`);
      setMessages(res.data.map(m => ({
        text: m.messageText,
        user: m.senderName === currentUser?.fullName ? 'You' : m.senderName || 'Anonymous',
        time: new Date(m.sentAt).toLocaleTimeString()
      })));
    } catch (err) {
      console.error('Error fetching messages', err);
    }
  };

  const fetchTodos = async () => {
    try {
      const res = await axios.get(`https://localhost:7183/api/ToDoItemsApi?roomId=${roomId}`);
      setTodoList(res.data.map(todo => ({
        id: todo.toDoItemId,
        task: todo.task,
        completed: todo.isCompleted
      })));
    } catch (err) {
      console.error('Error fetching todos', err);
    }
  };

  const draw = (e) => {
    if (!isDrawing.current) return;
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    const rect = canvas.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;
    ctx.lineTo(x, y);
    ctx.strokeStyle = tool === 'eraser' ? 'white' : color;
    ctx.lineWidth = tool === 'eraser' ? 10 : 2;
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(x, y);
  };

  const handleMouseDown = (e) => {
    isDrawing.current = true;
    draw(e);
  };

  const handleMouseUp = () => {
    isDrawing.current = false;
    canvasRef.current.getContext('2d').beginPath();
  };

  const handleSendMessage = async () => {
    if (!message.trim()) return;

    const senderName = currentUser?.fullName || 'You';

    const newMessage = {
      text: message,
      user: 'You',
      time: new Date().toLocaleTimeString()
    };
    setMessages(prev => [...prev, newMessage]);
    setMessage('');

    try {
      await axios.post('https://localhost:7183/api/ChatMessagesApi', {
        roomId: parseInt(roomId),
        senderName,
        messageText: message
      });
    } catch (err) {
      console.error('Error sending message', err);
    }
  };

  const handleTodoToggle = async (index) => {
    const updated = [...todoList];
    updated[index].completed = !updated[index].completed;
    setTodoList(updated);

    try {
      await axios.put(`https://localhost:7183/api/ToDoItemsApi/${updated[index].id}`, {
        ...updated[index],
        roomId: parseInt(roomId)
      });
    } catch (err) {
      console.error('Error updating todo', err);
    }
  };

  const handleAddTodo = async () => {
    if (!newTodo.trim()) return;

    try {
      await axios.post('https://localhost:7183/api/ToDoItemsApi', {
        roomId: parseInt(roomId),
        task: newTodo,
        isCompleted: false
      });
      setNewTodo('');
      fetchTodos();
    } catch (err) {
      console.error('Error adding todo:', err);
    }
  };

  const handleInvite = async () => {
    const roomLink = window.location.href;
    try {
      await axios.post('https://localhost:7183/api/ChatMessagesApi', {
        roomId: parseInt(roomId),
        senderName: 'System',
        messageText: `Join this room: ${roomLink}`
      });
      fetchMessages();
    } catch (err) {
      console.error('Failed to share invite link:', err);
    }
  };

  const handleLeaveRoom = async () => {
    try {
      await axios.delete(`https://localhost:7183/api/RoomMembersApi/leave?roomId=${roomId}&userId=${currentUser?.userId}`);
      navigate('/roommanagement');
    } catch (err) {
      console.error('Error leaving room:', err);
      alert('Failed to leave the room.');
    }
  };

  return (
    <div className="container py-4">
      <div className="d-flex justify-content-between align-items-center mb-4 p-3 bg-white rounded shadow-sm">
        <h2 className="mb-0">Study Room: {roomName}</h2>
        <div>
          <button className="btn btn-secondary me-2" onClick={() => navigate('/')}>⬅ Back</button>
          <button className="btn btn-primary me-2" onClick={handleInvite}><FaHandshake /> Invite</button>
          <button className="btn btn-danger" onClick={handleLeaveRoom}><FaBan /> Leave</button>
        </div>
      </div>

      <div className="d-flex gap-4">
        {/* Whiteboard, ToDo, Upload */}
        <div style={{ flex: 3 }}>
          {/* Canvas */}
          <div className="bg-white p-3 rounded shadow-sm mb-4">
            <h5>Whiteboard</h5>
            <div className="d-flex gap-3 mb-2">
              <select className="form-select w-auto" value={tool} onChange={e => setTool(e.target.value)}>
                <option value="pencil">Pencil</option>
                <option value="eraser">Eraser</option>
              </select>
              <input type="color" disabled={tool === 'eraser'} value={color} onChange={(e) => setColor(e.target.value)} />
            </div>
            <canvas
              ref={canvasRef}
              className="border rounded w-100"
              style={{ cursor: 'crosshair', height: '500px' }}
              onMouseDown={handleMouseDown}
              onMouseUp={handleMouseUp}
              onMouseMove={draw}
              onMouseLeave={handleMouseUp}
            />
          </div>

          {/* To-Do List */}
          <div className="bg-white p-3 rounded shadow-sm mb-3">
            <h5>To-Do List</h5>
            <div className="input-group mb-3">
              <input
                type="text"
                className="form-control"
                placeholder="Add a new task"
                value={newTodo}
                onChange={(e) => setNewTodo(e.target.value)}
              />
              <button className="btn btn-outline-primary" onClick={handleAddTodo}>Add</button>
            </div>
            <ul className="list-unstyled mb-0">
              {todoList.map((item, i) => (
                <li key={i} className="form-check mb-2">
                  <input
                    type="checkbox"
                    className="form-check-input"
                    id={`todo-${i}`}
                    checked={item.completed}
                    onChange={() => handleTodoToggle(i)}
                  />
                  <label htmlFor={`todo-${i}`} className="form-check-label">{item.task}</label>
                </li>
              ))}
            </ul>
          </div>

          {/* Upload */}
          <div className="bg-white p-3 rounded shadow-sm">
            <h5>Share Notes</h5>
            <label className="btn btn-success w-100">
              <FaPaperclip /> Upload
              <input type="file" hidden />
            </label>
          </div>
        </div>

        {/* Chat & Participants */}
        <div style={{ flex: 1, display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
          <div className="bg-white p-3 rounded shadow-sm d-flex flex-column flex-grow-1">
            <h5>Chat</h5>
            <div className="border p-2 rounded flex-grow-1 overflow-auto mb-2" style={{ minHeight: '300px' }}>
              {messages.map((m, idx) => (
                <div key={idx} className="mb-1">
                  <strong>{m.user}:</strong> {m.text}
                  <small className="text-muted ms-2">{m.time}</small>
                </div>
              ))}
            </div>
            <div className="input-group">
              <input type="text" className="form-control" value={message} onChange={(e) => setMessage(e.target.value)} />
              <button onClick={handleSendMessage} className="btn btn-primary">Send</button>
            </div>
          </div>

          <div className="bg-white p-3 rounded shadow-sm overflow-auto">
            <h5>Participants</h5>
            <ul className="list-unstyled">
              {participants.map(p => (
                <li key={p.id} className="mb-2">
                  {p.name}
                  <span className={`ms-2 badge ${p.online ? 'bg-success' : 'bg-secondary'}`}>
                    {p.online ? 'Online' : 'Offline'}
                  </span>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
};

export default StudyRoom;
