import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom"; // Get product ID from URL and navigate after updating
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"; // Import toastify CSS

const UpdateProduct = () => {
  const { id } = useParams(); // Get product ID from URL params
  const [productName, setProductName] = useState("");
  const [price, setPrice] = useState("");
  const [availableQuantity, setAvailableQuantity] = useState("");
  const [category, setCategory] = useState("");
  const [description, setDescription] = useState("");
  const [stockStatus, setStockStatus] = useState(2); // Default to "In Stock"
  const [categoryStatus, setCategoryStatus] = useState(1); // Default to "Active"
  const [loading, setLoading] = useState(true); // State for loading
  const navigate = useNavigate(); // Initialize navigate for redirection

  // Predefined category options
  const categoryOptions = [
    "Electronics",
    "Clothing",
    "Books",
    "Furniture",
    "Toys",
    "Home Appliances",
  ];

  // Predefined stock status options
  const stockStatusOptions = [
    { value: 0, label: "Out of Stock" },
    { value: 1, label: "Low Stock" },
    { value: 2, label: "In Stock" },
  ];

  // Predefined category status options
  const categoryStatusOptions = [
    { value: 0, label: "Inactive" },
    { value: 1, label: "Active" },
  ];

  useEffect(() => {
    const token = localStorage.getItem("token");
    console.log("Token:", token); // Log the token

    const role = localStorage.getItem("role");
    console.log("Role:", role); // Log the role

    // Fetch the existing product details when the component mounts
    const fetchProduct = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5282/api/product/${id}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        const product = response.data;
        // Populate state with existing product details
        setProductName(product.productName);
        setPrice(product.price);
        setAvailableQuantity(product.availableQuantity);
        setCategory(product.category);
        setDescription(product.description);
        setStockStatus(product.stockStatus);
        setCategoryStatus(product.categoryStatus);
        setLoading(false); // Set loading to false after fetching data
      } catch (error) {
        console.error("Failed to fetch product", error);
        toast.error("Failed to load product. Please try again.", {
          position: "top-right",
        });
        setLoading(false);
      }
    };

    fetchProduct(); // Call the fetchProduct function
  }, [id]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    const token = localStorage.getItem("token");
    console.log("Token at submission:", token); // Log the token at the time of form submission

    const role = localStorage.getItem("role");
    console.log("Role at submission:", role); // Log the role at submission (if necessary for validation)

    // Check if the required fields are filled out
    if (
      !productName ||
      !price ||
      !availableQuantity ||
      !category ||
      !description
    ) {
      toast.error("Please fill out all required fields.", {
        position: "top-right",
      });
      return;
    }

    const updatedProductData = {
      productName,
      price: parseFloat(price), // Ensure price is a float
      availableQuantity: parseInt(availableQuantity), // Ensure quantity is an integer
      category,
      description,
      stockStatus,
      categoryStatus,
    };

    try {
      const response = await axios.put(
        `http://localhost:5282/api/product/update/${id}`,
        updatedProductData,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.status === 200) {
        toast.success("Product updated successfully!", {
          position: "top-right",
        });

        // Redirect to the products page after a short delay
        setTimeout(() => {
          navigate("/products"); // Redirect to the products page
        }, 2000); // Delay for 2 seconds
      }
    } catch (error) {
      console.error("Failed to update product", error);
      toast.error("Failed to update product. Please try again.", {
        position: "top-right",
      });
    }
  };

  return (
    <div
      className="container mt-5"
      style={{
        maxWidth: "600px",
        padding: "20px",
        backgroundColor: "#f9f9f9",
        borderRadius: "8px",
        boxShadow: "0 0 10px rgba(0, 0, 0, 0.1)",
      }}
    >
      <h2 className="text-center mb-4" style={{ color: "#333" }}>
        Update Product
      </h2>

      {/* Loading state */}
      {loading ? (
        <p className="text-center">Loading product details...</p>
      ) : (
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Product Name</label>
            <input
              type="text"
              className="form-control"
              value={productName}
              onChange={(e) => setProductName(e.target.value)}
              required
              style={{ borderRadius: "4px", padding: "10px" }}
            />
          </div>

          <div className="form-group">
            <label>Price</label>
            <input
              type="number"
              className="form-control"
              step="0.01"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              required
              style={{ borderRadius: "4px", padding: "10px" }}
            />
          </div>

          <div className="form-group">
            <label>Available Quantity</label>
            <input
              type="number"
              className="form-control"
              value={availableQuantity}
              onChange={(e) => setAvailableQuantity(e.target.value)}
              required
              style={{ borderRadius: "4px", padding: "10px" }}
            />
          </div>

          <div className="form-group">
            <label>Category</label>
            <select
              className="form-control"
              value={category}
              onChange={(e) => setCategory(e.target.value)}
              required
              style={{ borderRadius: "4px", padding: "10px" }}
            >
              <option value="">Select a Category</option>
              {categoryOptions.map((categoryOption, index) => (
                <option key={index} value={categoryOption}>
                  {categoryOption}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Stock Status</label>
            <select
              className="form-control"
              value={stockStatus}
              onChange={(e) => setStockStatus(parseInt(e.target.value))}
              required
              style={{ borderRadius: "4px", padding: "10px" }}
            >
              {stockStatusOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Category Status</label>
            <select
              className="form-control"
              value={categoryStatus}
              onChange={(e) => setCategoryStatus(parseInt(e.target.value))}
              required
              style={{ borderRadius: "4px", padding: "10px" }}
            >
              {categoryStatusOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Description</label>
            <textarea
              className="form-control"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
              style={{
                borderRadius: "4px",
                padding: "10px",
                height: "100px",
                resize: "none",
              }}
            />
          </div>

          <div className="d-flex justify-content-center">
            <button
              type="submit"
              className="btn btn-primary mt-3"
              style={{
                backgroundColor: "#007bff",
                borderColor: "#007bff",
                padding: "12px 24px",
                fontSize: "16px",
              }}
            >
              Update Product
            </button>
          </div>
        </form>
      )}

      {/* Toast Container for notifications */}
      <ToastContainer />
    </div>
  );
};

export default UpdateProduct;
