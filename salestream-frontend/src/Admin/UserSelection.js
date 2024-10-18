import React from "react";
import { useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";

const UserSelection = () => {
  const navigate = useNavigate();

  // Handle role selection and navigation
  const handleSelection = (role) => {
    if (role === "vendor") {
      navigate("/vendor-login");
    } else if (role === "admin") {
      navigate("/log");
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
      <br></br>
    <div className="container mt-5 d-flex justify-content-center">
      <div className="card shadow p-5" style={{ maxWidth: "500px", width: "100%" }}>
        <h3 className="text-center mb-4">Select Your Role</h3>
        <div className="d-flex justify-content-around">
          {/* Vendor Button */}
          <button
            className="btn btn-primary w-45"
            onClick={() => handleSelection("vendor")}
          >
            Vendor
          </button>
          {/* Admin/CSR Button */}
          <button
            className="btn btn-secondary w-45"
            onClick={() => handleSelection("admin")}
          >
            Admin/CSR
          </button>
        </div>
      </div>
    </div>
    </div>
  );
};

export default UserSelection;
