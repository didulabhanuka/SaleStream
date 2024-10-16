import React, { useEffect, useState } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import "bootstrap/dist/css/bootstrap.min.css";
import { useNavigate } from "react-router-dom"; // Import useNavigate for navigation
import Navbar from '../components/usernavbar';

const VendorComment = () => {
  const navigate = useNavigate(); // Initialize navigate for navigation
  const [vendors, setVendors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [editingVendorId, setEditingVendorId] = useState(null);
  const [updatedVendor, setUpdatedVendor] = useState({
    vendorName: "",
    email: "",
    category: "",
  });

  // Function to fetch the list of vendors
  const fetchVendors = async () => {
    const token = localStorage.getItem("token");

    if (!token) {
      Swal.fire({
        icon: "error",
        title: "Authorization Error",
        text: "No token found. Please log in again.",
      });
      return;
    }

    try {
      const res = await axios.get("http://localhost:5282/api/Vendor/list", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setVendors(res.data);
    } catch (error) {
      console.error("Error fetching vendors: ", error);
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Failed to fetch vendor list! Please try again.",
      });
    } finally {
      setLoading(false);
    }
  };

  // Fetch vendors when the component mounts
  useEffect(() => {
    fetchVendors();
  }, []);

  const handleEditClick = (vendor) => {
    setEditingVendorId(vendor.id);
    setUpdatedVendor({
      vendorName: vendor.vendorName,
      email: vendor.email,
      category: vendor.category,
    });
  };

  const handleUpdateChange = (e) => {
    const { name, value } = e.target;
    setUpdatedVendor((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const isValidGmail = (email) => {
    // Regular expression to validate Gmail addresses
    const gmailRegex = /^[a-zA-Z0-9._%+-]+@gmail\.com$/;
    return gmailRegex.test(email);
  };

  const updateVendor = async () => {
    // Validate the Gmail address
    if (!isValidGmail(updatedVendor.email)) {
      Swal.fire({
        icon: "error",
        title: "Invalid Email",
        text: "Please enter a valid Gmail address.",
      });
      return;
    }

    const token = localStorage.getItem("token");

    try {
      await axios.put(`http://localhost:5282/api/Vendor/update/${editingVendorId}`, {
        ...updatedVendor,
        password: "newpassword123", // Use a real password or handle it securely
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      Swal.fire("Success!", "Vendor updated successfully", "success");
      // Refresh the vendor list after update
      fetchVendors();
      setEditingVendorId(null); // Exit edit mode
    } catch (error) {
      console.error("Error updating vendor: ", error);
      Swal.fire("Error!", "Failed to update vendor.", "error");
    }
  };

  const deleteVendor = async (email) => {
    const token = localStorage.getItem("token");

    const result = await Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`http://localhost:5282/api/Vendor/delete/${email}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        Swal.fire("Deleted!", "Vendor deleted successfully.", "success");
        // Refresh the vendor list after deletion
        fetchVendors();
      } catch (error) {
        console.error("Error deleting vendor: ", error);
        Swal.fire("Error!", "Failed to delete vendor.", "error");
      }
    }
  };

  const deactivateVendor = async (email) => {
    const token = localStorage.getItem("token");

    try {
      await axios.post(`http://localhost:5282/api/Vendor/deactivate/${email}`, {}, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      Swal.fire("Deactivated!", "Vendor deactivated successfully.", "success");
      // Refresh the vendor list after deactivation
      fetchVendors();
    } catch (error) {
      console.error("Error deactivating vendor: ", error);
      Swal.fire("Error!", "Failed to deactivate vendor.", "error");
    }
  };

  const activateVendor = async (email) => {
    const token = localStorage.getItem("token");

    try {
      await axios.post(`http://localhost:5282/api/Vendor/activate/${email}`, {}, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      Swal.fire("Activated!", "Vendor activated successfully.", "success");
      // Refresh the vendor list after activation
      fetchVendors();
    } catch (error) {
      console.error("Error activating vendor: ", error);
      Swal.fire("Error!", "Failed to activate vendor.", "error");
    }
  };

  const handleViewComments = (vendorId) => {
    navigate(`/vendor-comment/${vendorId}`); // Redirect to vendor comments page
  };

  return (
    <div>
      <Navbar />
      <div className="container mt-5">
        <h3 className="text-center mb-4">Vendor List</h3>
        {loading ? (
          <div className="text-center">
            <p>Loading vendors...</p>
          </div>
        ) : (
          <table className="table table-striped">
            <thead>
              <tr>
                <th>Vendor Name</th>
                <th>Email</th>
                <th>Category</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {vendors.length > 0 ? (
                vendors.map((vendor) => (
                  <tr key={vendor.id}>
                    <td>
                      {editingVendorId === vendor.id ? (
                        <input
                          type="text"
                          name="vendorName"
                          value={updatedVendor.vendorName}
                          onChange={handleUpdateChange}
                        />
                      ) : (
                        vendor.vendorName
                      )}
                    </td>
                    <td>
                      {editingVendorId === vendor.id ? (
                        <input
                          type="email"
                          name="email"
                          value={updatedVendor.email}
                          onChange={handleUpdateChange}
                        />
                      ) : (
                        vendor.email
                      )}
                    </td>
                    <td>
                      {editingVendorId === vendor.id ? (
                        <input
                          type="text"
                          name="category"
                          value={updatedVendor.category}
                          onChange={handleUpdateChange}
                        />
                      ) : (
                        vendor.category
                      )}
                    </td>
                    <td>{vendor.status === 1 ? "Active" : "Inactive"}</td>
                    <td>
                      {editingVendorId === vendor.id ? (
                        <button
                          className="btn btn-success btn-sm me-2"
                          onClick={updateVendor}
                        >
                          Confirm Update
                        </button>
                      ) : (
                        <button
                          className="btn btn-warning btn-sm me-2"
                          onClick={() => handleEditClick(vendor)}
                        >
                          Update
                        </button>
                      )}
                      <button
                        className="btn btn-danger btn-sm me-2"
                        onClick={() => deleteVendor(vendor.email)}
                      >
                        Delete
                      </button>
                      {vendor.status === 1 ? (
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={() => deactivateVendor(vendor.email)}
                        >
                          Deactivate
                        </button>
                      ) : (
                        <button
                          className="btn btn-success btn-sm"
                          onClick={() => activateVendor(vendor.email)}
                        >
                          Activate
                        </button>
                      )}
                      <button
                        className="btn btn-info btn-sm ms-2"
                        onClick={() => handleViewComments(vendor.id)}
                      >
                        View Comments
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5" className="text-center">
                    No vendors found.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
};

export default VendorComment;
