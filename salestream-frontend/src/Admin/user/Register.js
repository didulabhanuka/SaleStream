import React, { useState } from "react";
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap CSS

const Register = () => {
  // const [username, setUsername] = useState(""); // Uncomment if using username
  const [email, setEmail] = useState("");
  const [role, setRole] = useState("");
  const [password, setPassword] = useState("");
  const [rpassword, setRpassword] = useState("");
  const [isActive] = useState(true); // isActive is always true

  const navigate = useNavigate();

  const Submit = async (e) => {
    e.preventDefault();
  
    // Validate form fields
    if (!email || !role || !password || !rpassword) {
      Swal.fire({
        icon: "warning",
        title: "Warning",
        text: "All fields are required!",
      });
      return;
    }
  
    if (!/\S+@\S+\.\S+/.test(email)) {
      Swal.fire({
        icon: "warning",
        title: "Warning",
        text: "Invalid email format!",
      });
      return;
    }
  
    if (password.length < 6) {
      Swal.fire({
        icon: "warning",
        title: "Warning",
        text: "Password must be at least 6 characters long!",
      });
      return;
    }
  
    if (password !== rpassword) {
      Swal.fire({
        icon: "warning",
        title: "Warning",
        text: "Passwords do not match!",
      });
      return;
    }
  
    const newUser = {
      Email: email,          // Changed to match the expected casing
      Password: password,    // Changed to match the expected field name
      Role: role,           // Changed to match the expected field name
    };
  
    try {
      const res = await axios.post(
        "http://localhost:5282/api/Auth/register",
        newUser,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
  
      Swal.fire("Congrats!", "Successfully Registered", "success");
      navigate("/log");
    } catch (error) {
      console.error("Registration error: ", error);
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Registration failed! " + (error.response?.data?.message || "Please try again."),
      });
    }
  };
  

  return (
    <div className="container mt-5">
      <div className="d-flex justify-content-center">
        <div
          className="card shadow p-4"
          style={{ maxWidth: "500px", width: "100%" }}
        >
          <h3 className="text-center mb-4">Register</h3>
          <form onSubmit={Submit}>
            {/* Username */}
            {/* Uncomment if using username */}
            {/* <div className="mb-3">
              <label className="form-label">Username</label>
              <input
                type="text"
                className="form-control"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div> */}

            {/* Email */}
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                type="email"
                className="form-control"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            {/* Role */}
            <div className="mb-3">
              <label className="form-label">Role</label>
              <select
                className="form-select"
                value={role}
                onChange={(e) => setRole(e.target.value)}
              >
                <option value="">Choose</option>
                <option value="Admin">Admin</option>
                <option value="CSR">CSR</option>
              </select>
            </div>

            {/* Password */}
            <div className="mb-3">
              <label className="form-label">Password</label>
              <input
                type="password"
                className="form-control"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            {/* Confirm Password */}
            <div className="mb-3">
              <label className="form-label">Re-enter Password</label>
              <input
                type="password"
                className="form-control"
                value={rpassword}
                onChange={(e) => setRpassword(e.target.value)}
              />
            </div>

            {/* Submit Button */}
            <button type="submit" className="btn btn-primary w-100">
              Submit
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Register;
