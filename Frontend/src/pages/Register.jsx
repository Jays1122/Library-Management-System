import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosConfig';

const Register = () => {
    const [formData, setFormData] = useState({ name: '', email: '', password: '', role: 'Member' });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        setLoading(true); setError('');

        try {
            await axiosInstance.post('/Auth/register', formData);
            alert('Registration Successful! Please login.');
            navigate('/login');
        } catch (err) {
            // Backend se aane wale FluentValidation errors ko show karna
            if (err.response?.data?.errors) {
                setError(err.response.data.errors[0].error);
            } else {
                setError(err.response?.data?.message || 'Registration failed');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-black">
            <div className="bg-white/10 backdrop-blur-lg border border-white/20 p-10 rounded-2xl shadow-2xl w-full max-w-md">
                <div className="text-center mb-8">
                    <h2 className="text-4xl font-extrabold text-white tracking-tight">Join <span className="text-purple-400">Library</span></h2>
                    <p className="text-gray-300 mt-2 text-sm">Create your new account</p>
                </div>

                {error && <div className="bg-red-500/80 text-white p-3 rounded mb-5 text-sm text-center font-semibold">{error}</div>}

                <form onSubmit={handleRegister} className="space-y-5">
                    <input type="text" required placeholder="Full Name"
                        className="w-full px-4 py-3 bg-white/20 text-white placeholder-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-purple-400"
                        onChange={(e) => setFormData({ ...formData, name: e.target.value })} />

                    <input type="email" required placeholder="Email Address"
                        className="w-full px-4 py-3 bg-white/20 text-white placeholder-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-purple-400"
                        onChange={(e) => setFormData({ ...formData, email: e.target.value })} />

                    <input type="password" required placeholder="Strong Password"
                        className="w-full px-4 py-3 bg-white/20 text-white placeholder-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-purple-400"
                        onChange={(e) => setFormData({ ...formData, password: e.target.value })} />

                    <select className="w-full px-4 py-3 bg-white/20 text-white rounded-lg outline-none focus:ring-2 focus:ring-purple-400 [&>option]:text-black"
                        onChange={(e) => setFormData({ ...formData, role: e.target.value })}>
                        <option value="Member">Member</option>
                        <option value="Admin">Admin</option>
                    </select>

                    <button type="submit" disabled={loading}
                        className="w-full bg-purple-600 hover:bg-purple-700 text-white font-bold py-3 px-4 rounded-lg transition-all shadow-lg">
                        {loading ? 'Creating...' : 'Register'}
                    </button>
                </form>

                <div className="mt-6 text-center text-sm text-gray-300">
                    Already have an account? <span onClick={() => navigate('/login')} className="text-purple-400 font-bold cursor-pointer hover:underline">Sign In</span>
                </div>
            </div>
        </div>
    );
};

export default Register;