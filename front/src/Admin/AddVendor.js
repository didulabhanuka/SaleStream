import React, { useState } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../components/usernavbar';

const AddVendor = () => {
  const [vendorName, setVendorName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [category, setCategory] = useState("");

  const submitVendor = async (e) => {
    e.preventDefault();
  
    const newVendor = {
      VendorName: vendorName,
      Email: email,
      Password: password,
      Category: category,
    };
  
    const token = localStorage.getItem("token"); // Use "token" as in CustomerEdit
    console.log("Token retrieved:", token);
    
    if (!token) {
      Swal.fire({
        icon: "error",
        title: "Authorization Error",
        text: "No token found. Please log in again.",
      });
      return;
    }
  
    const tokenPayload = JSON.parse(atob(token.split('.')[1]));
    console.log("User Role:", tokenPayload.role); // Log user role
  
    try {
      console.log("New Vendor Data:", newVendor);
      console.log("Authorization Header:", `Bearer ${token}`);
  
      const res = await axios.post(
        "http://localhost:5282/api/Vendor/create",
        newVendor,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
  
      Swal.fire("Success!", "Vendor added successfully", "success");
      setVendorName("");
      setEmail("");
      setPassword("");
      setCategory("");
    } catch (error) {
      console.error("Error adding vendor: ", error);
      if (error.response) {
        if (error.response.status === 401) {
          Swal.fire({
            icon: "error",
            title: "Unauthorized",
            text: "You are not authorized to add a vendor. Please check your credentials and try again.",
          });
        } else {
          Swal.fire({
            icon: "error",
            title: "Error",
            text: "Failed to add vendor! " + (error.response.data.message || "Please try again."),
          });
        }
      } else {
        Swal.fire({
          icon: "error",
          title: "Error",
          text: "An unexpected error occurred. Please try again later.",
        });
      }
    }
  };
  
  return (
    <div>
      <Navbar />
      <div className="container mt-5">
        <div className="d-flex justify-content-center">
          <div
            className="card shadow p-4"
            style={{ maxWidth: "500px", width: "100%" }}
          >
            <h3 className="text-center mb-4">Add Vendor</h3>
            <form onSubmit={submitVendor}>
              {/* Vendor Name */}
              <div className="mb-3">
                <label className="form-label">Vendor Name</label>
                <input
                  type="text"
                  className="form-control"
                  value={vendorName}
                  onChange={(e) => setVendorName(e.target.value)}
                  required // Add validation for required fields
                />
              </div>

              {/* Email */}
              <div className="mb-3">
                <label className="form-label">Email</label>
                <input
                  type="email"
                  className="form-control"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required // Add validation for required fields
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
                  required // Add validation for required fields
                />
              </div>

              {/* Category */}
              <div className="mb-3">
                <label className="form-label">Category</label>
                <input
                  type="text"
                  className="form-control"
                  value={category}
                  onChange={(e) => setCategory(e.target.value)}
                  required // Add validation for required fields
                />
              </div>

              {/* Submit Button */}
              <button type="submit" className="btn btn-primary w-100">
                Add Vendor
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddVendor;
