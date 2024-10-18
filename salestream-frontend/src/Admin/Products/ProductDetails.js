import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"; // Import toastify CSS

const ProductDetails = () => {
  const { id } = useParams(); // Get product ID from URL
  const [product, setProduct] = useState(null); // State to hold the product details
  const navigate = useNavigate(); // Hook to navigate between pages

  useEffect(() => {
    // Fetch the product details by ID
    const fetchProductDetails = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5282/api/product/${id}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
        setProduct(response.data); // Set the product details
      } catch (error) {
        console.error("Failed to fetch product details", error);
        toast.error("Failed to load product details. Please try again.", {
          position: "top-right",
        });
      }
    };

    fetchProductDetails(); // Fetch product details
  }, [id]);

  return (
    <div>
      <br></br>
      <br></br>
      <br></br>
      <br></br>
      <br></br>
      <br></br>
    <div
      className="container mt-5"
      style={{
        maxWidth: "800px",
        padding: "30px",
        backgroundColor: "#fff",
        borderRadius: "12px",
        boxShadow: "0 10px 20px rgba(0, 0, 0, 0.1)",
        border: "1px solid #e3e3e3",
      }}
    >
      {product ? (
        <>
          <div
            style={{
              textAlign: "center",
              marginBottom: "20px",
              borderBottom: "2px solid #007bff",
              paddingBottom: "15px",
            }}
          >
            <h2 style={{ color: "#007bff", fontWeight: "bold" }}>
              {product.productName}
            </h2>
          </div>
          <div style={{ display: "flex", justifyContent: "space-between" }}>
            <p style={infoTextStyle}>
              <strong style={labelStyle}>Price:</strong> $
              {product.price.toFixed(2)}
            </p>
            <p style={infoTextStyle}>
              <strong style={labelStyle}>Quantity:</strong>{" "}
              {product.availableQuantity}
            </p>
          </div>
          <div style={{ marginBottom: "20px" }}>
            <p style={infoTextStyle}>
              <strong style={labelStyle}>Category:</strong> {product.category}
            </p>
            <p style={infoTextStyle}>
              <strong style={labelStyle}>Description:</strong>
              <span style={{ marginLeft: "10px" }}>{product.description}</span>
            </p>
          </div>

          {/* Back Button */}
          <button
            style={{
              backgroundColor: "#007bff",
              color: "#fff",
              padding: "10px 20px",
              borderRadius: "5px",
              border: "none",
              cursor: "pointer",
              fontSize: "16px",
              width: "100%",
              transition: "background-color 0.3s ease",
              marginTop: "20px",
            }}
            onMouseOver={(e) =>
              (e.currentTarget.style.backgroundColor = "#0056b3")
            }
            onMouseOut={(e) =>
              (e.currentTarget.style.backgroundColor = "#007bff")
            }
            onClick={() => navigate("/products")} // Redirect to products list
          >
            Back to Products List
          </button>
        </>
      ) : (
        <p style={{ textAlign: "center", fontSize: "18px" }}>
          Loading product details...
        </p>
      )}

      {/* Toast Container for notifications */}
      <ToastContainer />
    </div>
    </div>
  );
};

const infoTextStyle = {
  fontSize: "18px",
  color: "#333",
  marginBottom: "10px",
};

const labelStyle = {
  color: "#007bff",
  fontWeight: "600",
};

export default ProductDetails;
