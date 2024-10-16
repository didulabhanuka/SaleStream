import React, { useEffect, useState } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../components/usernavbar';

const CustomerEdit = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const token = localStorage.getItem("token");

                // Log the token to the console
                console.log("Token:", token);

                if (!token) {
                    throw new Error("Token is missing. Please log in.");
                }

                const res = await axios.get("http://localhost:5282/api/Auth/users", {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                const filteredUsers = res.data.filter(
                    (user) => user.role !== "Admin" && user.role !== "Vendor" && user.role !== "Customer Service Representative"
                );
                setUsers(filteredUsers);
            } catch (error) {
                console.error("Error fetching users: ", error);
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: error.response?.data?.message || error.message || "Failed to fetch user details.",
                });
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, []);

    const activateAccount = async (email) => {
        try {
            const token = localStorage.getItem("token");

            // Log the token to the console
            console.log("Token for activation:", token);

            if (!token) {
                throw new Error("Token is missing. Please log in.");
            }

            const res = await axios.post(
                "http://localhost:5282/api/Auth/activate",
                { email },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            Swal.fire("Success!", res.data || "Account activated successfully!", "success");
        } catch (error) {
            console.error("Error activating account: ", error);
            Swal.fire({
                icon: "error",
                title: "Error",
                text: error.response?.data?.message || error.message || "Failed to activate account.",
            });
        }
    };

    if (loading) {
        return <div className="text-center mt-5">Loading...</div>;
    }

    if (users.length === 0) {
        return <div className="text-center mt-5">No users found.</div>;
    }

    return (
        <div>
            <Navbar />
            <div
                className="container mt-5"
                style={{
                    backgroundColor: "#f8f9fa",
                    padding: "30px",
                    borderRadius: "10px",
                    boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)"
                }}
            >
                <h3
                    className="text-center mb-4"
                    style={{
                        color: "#343a40",
                        fontWeight: "700",
                        textTransform: "uppercase",
                        letterSpacing: "1px"
                    }}
                >
                    User List
                </h3>
                <table className="table table-striped table-hover">
                    <thead className="thead-dark">
                        <tr>
                            <th>Email</th>
                            <th>Role</th>
                            <th>Status</th>
                            <th>Activation</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user) => (
                            <tr key={user._id}>
                                <td>{user.email}</td>
                                <td>{user.role}</td>
                                <td
                                    style={{
                                        color: user.status === 1 ? "#28a745" : "#dc3545",
                                        fontWeight: "bold"
                                    }}
                                >
                                    {user.status === 1 ? "Active" : "Inactive"}
                                </td>
                                <td>
                                    {user.status === 0 && (
                                        <button
                                            className="btn btn-success"
                                            style={{
                                                backgroundColor: "#007bff",
                                                color: "white",
                                                fontSize: "14px",
                                                padding: "5px 10px",
                                                borderRadius: "5px",
                                                border: "none",
                                                transition: "background-color 0.3s ease"
                                            }}
                                            onClick={() => activateAccount(user.email)}
                                            onMouseOver={(e) => {
                                                e.target.style.backgroundColor = "#0056b3";
                                            }}
                                            onMouseOut={(e) => {
                                                e.target.style.backgroundColor = "#007bff";
                                            }}
                                        >
                                            Activate
                                        </button>
                                    )}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default CustomerEdit;
