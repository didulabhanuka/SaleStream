import React, { useEffect, useState } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import "bootstrap/dist/css/bootstrap.min.css";
import { useParams } from "react-router-dom";
import Navbar from '../components/usernavbar'; 

const VendorComment = () => {
  const { vendorId } = useParams(); // Get vendorId from URL parameters
  const [vendor, setVendor] = useState(null); // State to hold vendor details
  const [loading, setLoading] = useState(true); // Loading state

  // Function to fetch vendor details
  const fetchVendorDetails = async () => {
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
      console.log("Fetching vendor details for ID:", vendorId); // Log vendorId
      const res = await axios.get(`http://localhost:5282/api/vendor/${vendorId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      console.log("API Response:", res.data); // Log the full API response
      setVendor(res.data || null); // Set vendor state directly from the response data
    } catch (error) {
      console.error("Error fetching vendor details: ", error);
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Failed to fetch vendor details! Please try again.",
      });
    } finally {
      setLoading(false); // Set loading to false after fetching
    }
  };

  // Fetch vendor details when the component mounts
  useEffect(() => {
    fetchVendorDetails();
  }, [vendorId]);

  // Inline styles for the component
  const styles = {
    container: {
      marginTop: "50px",
      padding: "20px",
      borderRadius: "8px",
      boxShadow: "0 4px 8px rgba(0,0,0,0.1)",
      backgroundColor: "#f9f9f9",
    },
    header: {
      textAlign: "center",
      marginBottom: "30px",
      color: "#333",
    },
    card: {
      borderRadius: "8px",
      backgroundColor: "#fff",
      border: "1px solid #e0e0e0",
      boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
    },
    cardBody: {
      padding: "20px",
    },
    commentItem: {
      backgroundColor: "#f0f0f0",
      borderRadius: "5px",
      marginBottom: "10px",
      padding: "10px",
    },
    alert: {
      textAlign: "center",
    },
  };

  return (
    <div>
      <Navbar />
    <div style={styles.container}>
      <h3 style={styles.header}>Vendor Details for ID: {vendorId}</h3>
      {loading ? (
        <div className="text-center">Loading...</div> // Loading message
      ) : vendor ? (
        <div style={styles.card}>
          <div style={styles.cardBody}>
            <h4 className="card-title">{vendor.vendorName}</h4>
            <br></br> {/* Vendor Name */}
            <p className="card-text"><strong>Email:</strong> {vendor.email}</p> {/* Email */}
            <p className="card-text"><strong>Role:</strong> {vendor.role}</p> {/* Role */}
            <p className="card-text"><strong>Category:</strong> {vendor.category}</p> {/* Category */}
            <p className="card-text"><strong>Status:</strong> {vendor.status === 1 ? "Active" : "Inactive"}</p> {/* Status */}
            <h5>Comments:</h5>
            {vendor.comments && vendor.comments.length > 0 ? (
              <ul className="list-group">
                {vendor.comments.map((comment) => (
                  <li key={comment.id} className="list-group-item" style={styles.commentItem}>
                    <strong>User ID:</strong> {comment.userId} <br />
                    <strong>Comment:</strong> {comment.comment} <br />
                    <strong>Rank:</strong> {comment.rank} <br />
                  </li>
                ))}
              </ul>
            ) : (
              <div>No comments found.</div>
            )}
          </div>
        </div>
      ) : (
        <div className="alert alert-warning" style={styles.alert}>No vendor found with this ID.</div>
      )}
    </div>
    </div>
  );
};

export default VendorComment;
