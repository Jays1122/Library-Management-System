import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosConfig';

const Dashboard = () => {
    // Data States
    const [books, setBooks] = useState([]);
    const [issueRecords, setIssueRecords] = useState([]); // Naya state
    
    // UI States
    const [activeTab, setActiveTab] = useState('books'); // 'books' or 'records'
    const [searchQuery, setSearchQuery] = useState('');
    const [totalIssued, setTotalIssued] = useState(0);
    const [role, setRole] = useState('Member');
    const [userId, setUserId] = useState('');
    
    // Modal States
    const [showAddModal, setShowAddModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);
    const [formData, setFormData] = useState({ title: '', author: '', isbn: '', totalCopies: 1 });
    const [editBookId, setEditBookId] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (!token) { navigate('/login'); return; }
        
        const payload = JSON.parse(atob(token.split('.')[1]));
        const userRole = payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        const id = payload.nameid || payload.sub || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        
        setRole(userRole);
        setUserId(id);
        
        fetchBooks();
        fetchStats();
        if (userRole === 'Admin') fetchIssueRecords();
    }, []);

    // --- API CALLS ---
    const fetchBooks = async () => {
        try {
            const res = await axiosInstance.get('/Books');
            setBooks(res.data);
        } catch (error) { console.error("Error fetching books", error); }
    };

    const fetchStats = async () => {
        try {
            const res = await axiosInstance.get('/Books/stats/total-issued');
            setTotalIssued(res.data.totalIssued);
        } catch (error) { console.error("Error fetching stats", error); }
    };

    // NAYA: Issue Records Fetch Karna
    const fetchIssueRecords = async () => {
        try {
            const res = await axiosInstance.get('/Books/issue-records');
            setIssueRecords(res.data);
        } catch (error) { console.error("Error fetching records", error); }
    };

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!searchQuery) return fetchBooks();
        try {
            const res = await axiosInstance.get(`/Books/search?query=${searchQuery}`);
            setBooks(res.data);
        } catch (error) { console.error("Search failed", error); }
    };

    // --- ACTIONS ---
    const handleAddBook = async (e) => {
        e.preventDefault();
        try {
            await axiosInstance.post('/Books', formData);
            alert("Book Added!"); setShowAddModal(false); fetchBooks();
        } catch (error) { alert("Failed to add book"); }
    };

    const handleEditBook = async (e) => {
        e.preventDefault();
        try {
            await axiosInstance.put('/Books', { id: editBookId, ...formData });
            alert("Book Updated!"); setShowEditModal(false); fetchBooks();
        } catch (error) { alert("Failed to update book"); }
    };

    const handleDeleteBook = async (id) => {
        if (!window.confirm("Delete this book?")) return;
        try { await axiosInstance.delete(`/Books/${id}`); alert("Book Deleted!"); fetchBooks(); fetchStats(); } 
        catch (error) { alert("Failed to delete book"); }
    };

    const handleIssueBook = async (bookId) => {
        if (!window.confirm("Issue this book?")) return;
        try { 
            await axiosInstance.post('/Books/issue', { bookId, userId }); 
            alert("Book Issued!"); 
            fetchBooks(); fetchStats(); 
            if (role === 'Admin') fetchIssueRecords();
        } 
        catch (error) { alert(error.response?.data?.message || "Failed to issue"); }
    };

    // NAYA: Return Book Logic
    const handleReturnBook = async (issueRecordId) => {
        if (!window.confirm("Mark this book as Returned?")) return;
        try { 
            await axiosInstance.post('/Books/return', { issueRecordId }); 
            alert("Book Returned Successfully!"); 
            fetchBooks(); fetchStats(); fetchIssueRecords();
        } 
        catch (error) { alert(error.response?.data?.message || "Failed to return book"); }
    };

    const handleLogout = () => { localStorage.removeItem('token'); navigate('/login'); };

    const openEditModal = (book) => {
        setEditBookId(book.id);
        setFormData({ title: book.title, author: book.author, isbn: book.isbn, totalCopies: book.totalCopies });
        setShowEditModal(true);
    };

    return (
        <div className="min-h-screen bg-gray-50">
            {/* Navbar */}
            <nav className="bg-indigo-700 shadow-lg">
                <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
                    <h1 className="text-2xl font-extrabold text-white">📚 Library<span className="text-indigo-300">Pro</span></h1>
                    <div className="flex items-center gap-4">
                        <span className="text-white font-medium bg-indigo-800 px-3 py-1 rounded-full text-sm">{role} Mode</span>
                        <button onClick={handleLogout} className="bg-white text-indigo-700 font-bold px-5 py-2 rounded-lg shadow hover:bg-red-50 hover:text-red-600 transition-all">Logout</button>
                    </div>
                </div>
            </nav>

            <div className="max-w-7xl mx-auto px-6 py-8">
                {/* Stats */}
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
                    <div className="bg-white p-6 rounded-xl shadow-sm border-l-4 border-indigo-500">
                        <h3 className="text-gray-500 font-semibold text-sm">Total Books in Library</h3>
                        <p className="text-3xl font-bold text-gray-800 mt-2">{books.length}</p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow-sm border-l-4 border-orange-500">
                        <h3 className="text-gray-500 font-semibold text-sm">Currently Issued Books</h3>
                        <p className="text-3xl font-bold text-gray-800 mt-2">{totalIssued}</p>
                    </div>
                </div>

                {/* NAYA: Admin Tabs */}
                {role === 'Admin' && (
                    <div className="flex gap-4 mb-6 border-b border-gray-200 pb-2">
                        <button onClick={() => setActiveTab('books')} className={`font-bold px-4 py-2 rounded-t-lg transition-all ${activeTab === 'books' ? 'bg-indigo-600 text-white' : 'text-gray-600 hover:bg-gray-200'}`}>Manage Books</button>
                        <button onClick={() => setActiveTab('records')} className={`font-bold px-4 py-2 rounded-t-lg transition-all ${activeTab === 'records' ? 'bg-indigo-600 text-white' : 'text-gray-600 hover:bg-gray-200'}`}>Issue & Return Records</button>
                    </div>
                )}

                {/* --- TAB 1: MANAGE BOOKS --- */}
                {activeTab === 'books' && (
                    <>
                        {/* Search & Add Button */}
                        <div className="flex flex-col md:flex-row justify-between items-center gap-4 mb-6">
                            <form onSubmit={handleSearch} className="w-full md:w-1/2 flex">
                                <input type="text" placeholder="Search by Title or Author..." className="w-full px-4 py-3 rounded-l-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500" value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} />
                                <button type="submit" className="bg-indigo-600 text-white px-6 py-3 rounded-r-lg hover:bg-indigo-700 font-semibold transition-all">Search</button>
                            </form>
                            {role === 'Admin' && (
                                <button onClick={() => { setFormData({ title: '', author: '', isbn: '', totalCopies: 1 }); setShowAddModal(true); }} className="bg-green-600 text-white px-6 py-3 rounded-lg font-bold shadow-lg hover:bg-green-700 transition-all">
                                    + Add New Book
                                </button>
                            )}
                        </div>

                        {/* Book Table */}
                        <div className="bg-white rounded-xl shadow-md overflow-hidden border border-gray-100">
                            <table className="w-full text-left border-collapse">
                                <thead>
                                    <tr className="bg-gray-100 text-gray-600 uppercase text-sm leading-normal">
                                        <th className="py-4 px-6 font-bold">Title</th>
                                        <th className="py-4 px-6 font-bold">Author</th>
                                        <th className="py-4 px-6 font-bold text-center">Total Copies</th>
                                        <th className="py-4 px-6 font-bold text-center">Available</th>
                                        <th className="py-4 px-6 font-bold text-center">Actions</th>
                                    </tr>
                                </thead>
                                <tbody className="text-gray-700 text-sm">
                                    {books.length === 0 ? ( <tr><td colSpan="5" className="text-center py-8 text-gray-500 text-lg">No Books Found</td></tr>) : books.map((book) => (
                                        <tr key={book.id} className="border-b border-gray-200 hover:bg-gray-50 transition-all">
                                            <td className="py-4 px-6 font-medium text-gray-900">{book.title}</td>
                                            <td className="py-4 px-6">{book.author}</td>
                                            <td className="py-4 px-6 text-center"><span className="bg-blue-100 text-blue-700 py-1 px-3 rounded-full text-xs font-bold">{book.totalCopies}</span></td>
                                            <td className="py-4 px-6 text-center"><span className={`py-1 px-3 rounded-full text-xs font-bold ${book.availableCopies > 0 ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>{book.availableCopies}</span></td>
                                            <td className="py-4 px-6 flex justify-center gap-2">
                                                {book.availableCopies > 0 ? (
                                                    <button onClick={() => handleIssueBook(book.id)} className="bg-indigo-100 text-indigo-700 font-semibold py-1 px-3 rounded hover:bg-indigo-200 transition-all">Issue</button>
                                                ) : (<span className="text-red-500 font-semibold text-sm py-1 px-3">Out of Stock</span>)}
                                                {role === 'Admin' && (
                                                    <>
                                                        <button onClick={() => openEditModal(book)} className="bg-yellow-100 text-yellow-700 font-semibold py-1 px-3 rounded hover:bg-yellow-200 transition-all">Edit</button>
                                                        <button onClick={() => handleDeleteBook(book.id)} className="bg-red-100 text-red-700 font-semibold py-1 px-3 rounded hover:bg-red-200 transition-all">Delete</button>
                                                    </>
                                                )}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </>
                )}

                {/* --- TAB 2: ISSUE & RETURN RECORDS (Admin Only) --- */}
                {activeTab === 'records' && role === 'Admin' && (
                    <div className="bg-white rounded-xl shadow-md overflow-hidden border border-gray-100">
                        <table className="w-full text-left border-collapse">
                            <thead>
                                <tr className="bg-gray-100 text-gray-600 uppercase text-sm leading-normal">
                                    <th className="py-4 px-6 font-bold">Record ID</th>
                                    <th className="py-4 px-6 font-bold">Issue Date</th>
                                    <th className="py-4 px-6 font-bold">Status</th>
                                    <th className="py-4 px-6 font-bold text-center">Action</th>
                                </tr>
                            </thead>
                            <tbody className="text-gray-700 text-sm">
                                {issueRecords.length === 0 ? ( <tr><td colSpan="4" className="text-center py-8 text-gray-500 text-lg">No Records Found</td></tr>) : issueRecords.map((record) => (
                                    <tr key={record.id} className="border-b border-gray-200 hover:bg-gray-50 transition-all">
                                        <td className="py-4 px-6 font-medium text-gray-900 text-xs">{record.id}</td>
                                        <td className="py-4 px-6">{new Date(record.issueDate).toLocaleDateString()}</td>
                                        <td className="py-4 px-6">
                                            {record.isReturned 
                                                ? <span className="bg-green-100 text-green-700 py-1 px-3 rounded-full text-xs font-bold">Returned ({new Date(record.returnDate).toLocaleDateString()})</span>
                                                : <span className="bg-orange-100 text-orange-700 py-1 px-3 rounded-full text-xs font-bold">Issued (Not Returned)</span>
                                            }
                                        </td>
                                        <td className="py-4 px-6 text-center">
                                            {!record.isReturned ? (
                                                <button onClick={() => handleReturnBook(record.id)} className="bg-blue-600 text-white font-semibold py-1 px-4 rounded hover:bg-blue-700 transition-all shadow">
                                                    Mark as Returned
                                                </button>
                                            ) : (
                                                <span className="text-gray-400 font-semibold">Done</span>
                                            )}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>

            {/* --- MODALS (Add & Edit) --- */}
            {(showAddModal || showEditModal) && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
                    <div className="bg-white p-8 rounded-2xl shadow-2xl w-full max-w-md transform transition-all">
                        <h2 className="text-2xl font-bold text-gray-800 mb-6">{showAddModal ? 'Add New Book' : 'Edit Book'}</h2>
                        <form onSubmit={showAddModal ? handleAddBook : handleEditBook} className="space-y-4">
                            <input type="text" placeholder="Book Title" required className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none" value={formData.title} onChange={(e) => setFormData({ ...formData, title: e.target.value })} />
                            <input type="text" placeholder="Author Name" required className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none" value={formData.author} onChange={(e) => setFormData({ ...formData, author: e.target.value })} />
                            <input type="text" placeholder="ISBN (Optional)" className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none" value={formData.isbn} onChange={(e) => setFormData({ ...formData, isbn: e.target.value })} />
                            <input type="number" min="1" placeholder="Total Copies" required className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none" value={formData.totalCopies} onChange={(e) => setFormData({ ...formData, totalCopies: parseInt(e.target.value) })} />
                            
                            <div className="flex justify-end gap-3 mt-6">
                                <button type="button" onClick={() => { setShowAddModal(false); setShowEditModal(false); }} className="bg-gray-200 text-gray-700 px-4 py-2 rounded-lg font-semibold hover:bg-gray-300">Cancel</button>
                                <button type="submit" className="bg-indigo-600 text-white px-4 py-2 rounded-lg font-bold hover:bg-indigo-700 shadow-lg">{showAddModal ? 'Add Book' : 'Save Changes'}</button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Dashboard;