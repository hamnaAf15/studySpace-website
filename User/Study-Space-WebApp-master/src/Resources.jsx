import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './resources.css';
import {
  FaFilePdf,
  FaVideo,
  FaBook,
  FaSearch,
  FaUpload,
  FaTrash,
  FaDownload
} from 'react-icons/fa';

const Resources = () => {
  const [resources, setResources] = useState([]);
  const [activeTab, setActiveTab] = useState('books');
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedFile, setSelectedFile] = useState(null);
  const [newTitle, setNewTitle] = useState('');
  const [newType, setNewType] = useState('book');

  useEffect(() => {
    fetchResources();
  }, []);

  const fetchResources = () => {
    axios.get('https://localhost:7183/api/ResourcesApi')
      .then(res => setResources(res.data))
      .catch(err => console.error('Error fetching resources:', err));
  };

  const handleUpload = async () => {
    if (!selectedFile || !newTitle || !newType) {
      alert('Please fill all fields and select a file.');
      return;
    }

    const formData = new FormData();
    formData.append('File', selectedFile);
    formData.append('Title', newTitle);
    formData.append('Type', newType);

    try {
      await axios.post('https://localhost:7183/api/ResourcesApi/upload', formData);
      fetchResources(); // Refresh after upload
      setSelectedFile(null);
      setNewTitle('');
      setNewType('book');
      alert('Resource uploaded successfully!');
    } catch (err) {
      console.error('Upload error:', err);
      alert('Failed to upload resource');
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this resource?')) {
      try {
        await axios.delete(`https://localhost:7183/api/ResourcesApi/${id}`);
        setResources(prev => prev.filter(r => r.resourceId !== id));
      } catch (err) {
        console.error('Delete error:', err);
      }
    }
  };

  const getIcon = (type) => {
    switch (type.toLowerCase()) {
      case 'book': return <FaBook className="text-primary me-2" />;
      case 'pdf': return <FaFilePdf className="text-danger me-2" />;
      case 'lecture': return <FaVideo className="text-secondary me-2" />;
      default: return null;
    }
  };

  const filteredResources = resources
    .filter(r => r.fileType?.toLowerCase() === activeTab.slice(0, -1)) // books → book
    .filter(r => r.title?.toLowerCase().includes(searchQuery.toLowerCase()));

  const typeCounts = {
    books: resources.filter(r => r.fileType?.toLowerCase() === 'book').length,
    pdfs: resources.filter(r => r.fileType?.toLowerCase() === 'pdf').length,
    lectures: resources.filter(r => r.fileType?.toLowerCase() === 'lecture').length,
  };

  const renderCards = () =>
    filteredResources.map((resource, index) => (
      <div key={resource.resourceId || index} className="col-md-4 d-flex">
        <div className="card resource-card shadow-sm w-100">
          <div className="card-body d-flex flex-column justify-content-between">
            <div className="d-flex align-items-center mb-3">
              {getIcon(resource.fileType)}
              <h6 className="card-title mb-0">{resource.title}</h6>
            </div>
            <div className="d-flex justify-content-end gap-2">
              <a href={resource.fileUrl} className="btn btn-sm btn-outline-primary" title="Download" download>
                <FaDownload />
              </a>
              <button
                className="btn btn-sm btn-outline-danger"
                title="Delete"
                onClick={() => handleDelete(resource.resourceId)}
              >
                <FaTrash />
              </button>
            </div>
          </div>
        </div>
      </div>
    ));

  return (
    <div className="container resources-page">
      {/* Title */}
      <div className="resources-header mb-4">
        <h2 className="resources-title">Resources</h2>
      </div>

      {/* Upload Section */}
      <div className="row mb-4">
        <div className="col-md-3">
          <input type="file" className="form-control" onChange={(e) => setSelectedFile(e.target.files[0])} />
        </div>
        <div className="col-md-3">
          <input type="text" className="form-control" placeholder="Resource Title" value={newTitle} onChange={(e) => setNewTitle(e.target.value)} />
        </div>
        <div className="col-md-3">
          <select className="form-select" value={newType} onChange={(e) => setNewType(e.target.value)}>
            <option value="book">Book</option>
            <option value="pdf">PDF</option>
            <option value="lecture">Lecture</option>
          </select>
        </div>
        <div className="col-md-3 d-flex align-items-end">
          <button className="btn btn-primary w-100" onClick={handleUpload}>
            <FaUpload className="me-2" /> Upload Resource
          </button>
        </div>
      </div>

      {/* Search */}
      <div className="input-group mb-4 shadow-sm">
        <span className="input-group-text bg-light"><FaSearch /></span>
        <input
          type="text"
          className="form-control"
          placeholder="Search your resources..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </div>

      {/* Stats */}
      <div className="row text-center mb-4">
        <div className="col">
          <div className="stat-box bg-primary text-white rounded p-3">
            <h5>{typeCounts.books}</h5>
            <p className="mb-0">Books</p>
          </div>
        </div>
        <div className="col">
          <div className="stat-box bg-danger text-white rounded p-3">
            <h5>{typeCounts.pdfs}</h5>
            <p className="mb-0">PDFs</p>
          </div>
        </div>
        <div className="col">
          <div className="stat-box bg-secondary text-white rounded p-3">
            <h5>{typeCounts.lectures}</h5>
            <p className="mb-0">Lectures</p>
          </div>
        </div>
      </div>

      {/* Tabs */}
      <ul className="nav nav-tabs mb-3">
        {['books', 'pdfs', 'lectures'].map((tab) => (
          <li className="nav-item" key={tab}>
            <button
              className={`nav-link ${activeTab === tab ? 'active' : ''}`}
              onClick={() => setActiveTab(tab)}
            >
              {tab.charAt(0).toUpperCase() + tab.slice(1)}
            </button>
          </li>
        ))}
      </ul>

      {/* Resource Cards */}
      <div className="row g-4">
        {renderCards()}
      </div>
    </div>
  );
};

export default Resources;
