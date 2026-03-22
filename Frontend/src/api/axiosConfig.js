import axios from 'axios';

// NOTE: Apna Backend ka Swagger wala base URL yahan daalein (Bina /api/Auth ke)
// Example: https://localhost:7123/api
const API_URL = 'https://localhost:7278/LMS'; 

const axiosInstance = axios.create({
    baseURL: API_URL,
});

// Ye function har API call se pehle Token attach kar dega
axiosInstance.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default axiosInstance;