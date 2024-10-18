// import React, { useState } from "react";
// import axios from "axios";

// const AddProduct = () => {
//   const [productName, setProductName] = useState("");
//   const [price, setPrice] = useState("");
//   const [availableQuantity, setAvailableQuantity] = useState("");
//   const [category, setCategory] = useState("");
//   const [description, setDescription] = useState("");
//   const [message, setMessage] = useState("");

//   // Assuming the token is stored in localStorage after user login
//   const token = localStorage.getItem("token");

//   const handleSubmit = async (event) => {
//     event.preventDefault();

//     const productData = {
//       productName,
//       price: parseFloat(price),
//       availableQuantity: parseInt(availableQuantity),
//       category,
//       description,
//     };

//     try {
//       const response = await axios.post(
//         "http://localhost:5282/api/product/create",
//         productData,
//         {
//           headers: {
//             "Content-Type": "application/json",
//             Authorization: `Bearer ${token}`, // Attach the bearer token
//           },
//         }
//       );

//       if (response.status === 200) {
//         setMessage("Product added successfully!");
//         setProductName("");
//         setPrice("");
//         setAvailableQuantity("");
//         setCategory("");
//         setDescription("");
//       }
//     } catch (error) {
//       console.error("There was an error adding the product!", error);
//       setMessage("Failed to add the product. Please try again.");
//     }
//   };

//   return (
//     <div>
//       <h2>Add New Product</h2>
//       <form onSubmit={handleSubmit}>
//         <div>
//           <label>Product Name:</label>
//           <input
//             type="text"
//             value={productName}
//             onChange={(e) => setProductName(e.target.value)}
//             required
//           />
//         </div>

//         <div>
//           <label>Price:</label>
//           <input
//             type="number"
//             step="0.01"
//             value={price}
//             onChange={(e) => setPrice(e.target.value)}
//             required
//           />
//         </div>

//         <div>
//           <label>Available Quantity:</label>
//           <input
//             type="number"
//             value={availableQuantity}
//             onChange={(e) => setAvailableQuantity(e.target.value)}
//             required
//           />
//         </div>

//         <div>
//           <label>Category:</label>
//           <input
//             type="text"
//             value={category}
//             onChange={(e) => setCategory(e.target.value)}
//             required
//           />
//         </div>

//         <div>
//           <label>Description:</label>
//           <textarea
//             value={description}
//             onChange={(e) => setDescription(e.target.value)}
//             required
//           />
//         </div>

//         <button type="submit">Add Product</button>
//       </form>

//       {message && <p>{message}</p>}
//     </div>
//   );
// };

// export default AddProduct;

import React, { useState } from "react";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"; // Import the CSS for the toast
import { useNavigate } from "react-router-dom"; // Import useNavigate from react-router-dom

const AddProduct = () => {
  const [productName, setProductName] = useState("");
  const [price, setPrice] = useState("");
  const [availableQuantity, setAvailableQuantity] = useState("");
  const [category, setCategory] = useState(""); // Category will be selected from dropdown
  const [description, setDescription] = useState("");
  const [stockStatus, setStockStatus] = useState(2); // Default to "In Stock"
  const [categoryStatus, setCategoryStatus] = useState(1); // Default to "Active"

  const navigate = useNavigate(); // Initialize the useNavigate hook for redirection

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

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!category) {
      toast.error("Please select a category!", {
        position: "top-right",
      });
      return;
    }

    const vendorEmail =
      localStorage.getItem("email") || "defaultVendor@example.com"; // Fetch the vendor's email from localStorage or set a default

    const productData = {
      productName,
      price: parseFloat(price),
      availableQuantity: parseInt(availableQuantity),
      category,
      description,
      stockStatus,
      categoryStatus,
      vendorEmail,
      lowStockStatusNotificationDateAndTime: new Date().toISOString(), // Automatically set the date
    };

    try {
      const response = await axios.post(
        "http://localhost:5282/api/product/create",
        productData,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );

      if (response.status === 200) {
        toast.success("Product added successfully!", {
          position: "top-right",
        });

        // Reset form fields
        setProductName("");
        setPrice("");
        setAvailableQuantity("");
        setCategory("");
        setDescription("");
        setStockStatus(2); // Reset to "In Stock"
        setCategoryStatus(1); // Reset to "Active"

        // Navigate to the products page after a short delay to allow the toast message to display
        setTimeout(() => {
          navigate("/products"); // Replace with the actual path to your products page
        }, 2000); // Redirect after 2 seconds
      }
    } catch (error) {
      toast.error("Failed to add the product. Please try again.", {
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
        Add New Product
      </h2>
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
            Add Product
          </button>
        </div>
      </form>

      {/* Toast Container */}
      <ToastContainer />
    </div>
  );
};

export default AddProduct;
