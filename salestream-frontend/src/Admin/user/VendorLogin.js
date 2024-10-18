import React, { useState } from "react";
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import "bootstrap/dist/css/bootstrap.min.css";

const VendorLogin = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false); // Loading state
    const navigate = useNavigate();

    const Submit = async (e) => {
        e.preventDefault();
        setLoading(true);
    
        const loginVendor = {
            Email: email,
            Password: password,
        };
    
        try {
            const res = await axios.post(
                "http://localhost:5282/api/Vendor/login",
                loginVendor,
                {
                    headers: {
                        "Content-Type": "application/json",
                    },
                }
            );
    
            console.log("Login response:", res.data); // Log the entire response
    
            const { token, role } = res.data; // Ensure the destructuring matches the response structure
            console.log("Token received:", token); // Log the token for debugging
            console.log("Role received:", role); // Log the role for debugging
            
            Swal.fire("Success!", "Login successful", "success");
            localStorage.setItem("token", token); // Store the token in local storage
    
            if (role === "Vendor") {
                navigate("/products"); // Redirect to vendor dashboard
            } else {
                navigate("/"); // Just in case
            }
        } catch (error) {
            console.error("Login error: ", error);
            const errorMessage = error.response?.data?.message || "Login failed! Please check your credentials.";
            Swal.fire({
                icon: "error",
                title: "Error",
                text: errorMessage,
            });
        } finally {
            setLoading(false);
        }
    };
    
    return (
        <div>
            <br></br>
            <br></br>
            <br></br>
            <br></br>
            <br></br>
            <br></br>
            <div className="container mt-5">
                <div className="d-flex justify-content-center">
                    <div className="card shadow p-4" style={{ maxWidth: "500px", width: "100%" }}>
                        <h3 className="text-center mb-4">Vendor Login</h3>
                        <form onSubmit={Submit}>
                            {/* Email */}
                            <div className="mb-3">
                                <label className="form-label">Email</label>
                                <input
                                    type="email"
                                    className="form-control"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>

                            {/* Password */}
                            <div className="mb-3">
                                <label className="form-label">Password</label>
                                <input
                                    type="password"
                                    className="form-control"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                            </div>

                            {/* Submit Button */}
                            <button type="submit" className="btn btn-primary w-100" disabled={loading}>
                                {loading ? "Logging in..." : "Login"}
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default VendorLogin;
