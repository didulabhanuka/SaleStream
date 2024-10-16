// import React, { useState, useEffect } from "react";
// import axios from "axios";
// import { ToastContainer, toast } from "react-toastify";
// import "react-toastify/dist/ReactToastify.css"; // Import toastify CSS
// import { useNavigate } from "react-router-dom";

// const ProductsPage = () => {
//   const [products, setProducts] = useState([]); // State to hold fetched products
//   const [loading, setLoading] = useState(true); // State for loading
//   const navigate = useNavigate(); // For navigation

//   useEffect(() => {
//     // Function to fetch products
//     const fetchProducts = async () => {
//       try {
//         const response = await axios.get(
//           "http://localhost:5282/api/product/all",
//           {
//             headers: {
//               Authorization: `Bearer ${localStorage.getItem("token")}`,
//             },
//           }
//         );
//         setProducts(response.data); // Set the products from the response
//         setLoading(false); // Set loading to false once products are fetched
//       } catch (error) {
//         console.error("Failed to fetch products", error);
//         toast.error("Failed to fetch products. Please try again.", {
//           position: "top-right",
//         });
//         setLoading(false);
//       }
//     };

//     fetchProducts(); // Call the function to fetch products
//   }, []); // Empty dependency array ensures this effect runs only once on mount

//   // Truncate description to a specific length
//   const truncateDescription = (description, maxLength) => {
//     if (description.length > maxLength) {
//       return description.substring(0, maxLength) + "...";
//     }
//     return description;
//   };

//   // Handle navigation to product details page
//   const handleViewMore = (id) => {
//     navigate(`/product-details/${id}`);
//   };

//   // Handle update action (redirect to update form/page)
//   const handleUpdate = (id) => {
//     // Navigate to the update page with the product ID
//     navigate(`/update-product/${id}`);
//   };

//   // Handle delete action
//   const handleDelete = async (id) => {
//     try {
//       await axios.delete(`http://localhost:5282/api/product/delete/${id}`, {
//         headers: {
//           Authorization: `Bearer ${localStorage.getItem("token")}`,
//         },
//       });
//       setProducts(products.filter((product) => product.id !== id)); // Remove product from state
//       toast.success("Product deleted successfully.", {
//         position: "top-center",
//       });
//     } catch (error) {
//       console.error("Failed to delete product", error);
//       toast.error("Failed to delete product. Please try again.", {
//         position: "top-center",
//       });
//     }
//   };

//   // Custom confirmation with Toastify
//   const confirmDelete = (id) => {
//     toast(
//       ({ closeToast }) => (
//         <div>
//           <p>Are you sure you want to delete this product?</p>
//           <div>
//             <button
//               style={{
//                 backgroundColor: "#dc3545",
//                 color: "#fff",
//                 padding: "5px 10px",
//                 border: "none",
//                 borderRadius: "3px",
//                 marginRight: "5px",
//                 cursor: "pointer",
//               }}
//               onClick={() => {
//                 handleDelete(id);
//                 closeToast();
//               }}
//             >
//               Yes, Delete
//             </button>
//             <button
//               style={{
//                 backgroundColor: "#007bff",
//                 color: "#fff",
//                 padding: "5px 10px",
//                 border: "none",
//                 borderRadius: "3px",
//                 cursor: "pointer",
//               }}
//               onClick={closeToast}
//             >
//               Cancel
//             </button>
//           </div>
//         </div>
//       ),
//       {
//         position: "top-center", // Position the toast at the top center
//         autoClose: false, // Disable auto-close for confirmation
//         closeOnClick: false,
//         closeButton: false,
//       }
//     );
//   };

//   // JSX to render the product list or a loading message
//   return (
//     <div
//       className="container mt-5"
//       style={{
//         maxWidth: "900px",
//         padding: "20px",
//         backgroundColor: "#f9f9f9",
//         borderRadius: "10px",
//         boxShadow: "0 4px 10px rgba(0, 0, 0, 0.1)",
//       }}
//     >
//       <h2 className="text-center mb-4" style={{ color: "#333" }}>
//         Available Products
//       </h2>

//       {/* If loading, show a loading message */}
//       {loading ? (
//         <p className="text-center">Loading products...</p>
//       ) : (
//         <>
//           {/* If no products available, show a message */}
//           {products.length === 0 ? (
//             <p className="text-center">No products available.</p>
//           ) : (
//             <table className="table table-bordered table-hover">
//               <thead className="thead-light">
//                 <tr>
//                   <th>Product Name</th>
//                   <th>Price</th>
//                   <th>Available Quantity</th>
//                   <th>Category</th>
//                   <th>Description</th>
//                   <th className="text-center">Actions</th>
//                 </tr>
//               </thead>
//               <tbody>
//                 {products.map((product) => (
//                   <tr key={product.id}>
//                     <td>{product.productName}</td>
//                     <td>${product.price.toFixed(2)}</td>
//                     <td>{product.availableQuantity}</td>
//                     <td>{product.category}</td>
//                     <td>{truncateDescription(product.description, 50)}</td>
//                     <td
//                       className="text-center"
//                       style={{ verticalAlign: "middle" }}
//                     >
//                       <div
//                         style={{
//                           display: "flex",
//                           flexDirection: "column",
//                           alignItems: "center",
//                         }}
//                       >
//                         <button
//                           style={{
//                             backgroundColor: "#007bff",
//                             color: "#fff",
//                             padding: "8px 15px",
//                             fontSize: "14px",
//                             border: "none",
//                             borderRadius: "5px",
//                             marginBottom: "5px", // Add spacing between buttons
//                             transition: "background-color 0.3s ease",
//                             width: "100px", // Ensure all buttons are the same width
//                             cursor: "pointer",
//                           }}
//                           onClick={() => handleViewMore(product.id)}
//                         >
//                           View More
//                         </button>
//                         <button
//                           style={{
//                             backgroundColor: "#ffc107",
//                             color: "#fff",
//                             padding: "8px 15px",
//                             fontSize: "14px",
//                             border: "none",
//                             borderRadius: "5px",
//                             marginBottom: "5px", // Add spacing between buttons
//                             transition: "background-color 0.3s ease",
//                             width: "100px", // Ensure all buttons are the same width
//                             cursor: "pointer",
//                           }}
//                           onMouseOver={(e) =>
//                             (e.currentTarget.style.backgroundColor = "#e0a800")
//                           }
//                           onMouseOut={(e) =>
//                             (e.currentTarget.style.backgroundColor = "#ffc107")
//                           }
//                           onClick={() => handleUpdate(product.id)}
//                         >
//                           Update
//                         </button>
//                         <button
//                           style={{
//                             backgroundColor: "#dc3545",
//                             color: "#fff",
//                             padding: "8px 15px",
//                             fontSize: "14px",
//                             border: "none",
//                             borderRadius: "5px",
//                             transition: "background-color 0.3s ease",
//                             width: "100px", // Ensure all buttons are the same width
//                             cursor: "pointer",
//                           }}
//                           onMouseOver={(e) =>
//                             (e.currentTarget.style.backgroundColor = "#c82333")
//                           }
//                           onMouseOut={(e) =>
//                             (e.currentTarget.style.backgroundColor = "#dc3545")
//                           }
//                           onClick={() => confirmDelete(product.id)}
//                         >
//                           Delete
//                         </button>
//                       </div>
//                     </td>
//                   </tr>
//                 ))}
//               </tbody>
//             </table>
//           )}
//         </>
//       )}

//       {/* Toast Container for notifications */}
//       <ToastContainer />
//     </div>
//   );
// };

// export default ProductsPage;

import React, { useState, useEffect } from "react";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"; // Import toastify CSS
import { useNavigate } from "react-router-dom";

const ProductsPage = () => {
  const [products, setProducts] = useState([]); // State to hold fetched products
  const [loading, setLoading] = useState(true); // State for loading
  const navigate = useNavigate(); // For navigation

  useEffect(() => {
    // Function to fetch products
    const fetchProducts = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5282/api/product/all",
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
        setProducts(response.data); // Set the products from the response
        setLoading(false); // Set loading to false once products are fetched
      } catch (error) {
        console.error("Failed to fetch products", error);
        toast.error("Failed to fetch products. Please try again.", {
          position: "top-right",
        });
        setLoading(false);
      }
    };

    fetchProducts(); // Call the function to fetch products
  }, []); // Empty dependency array ensures this effect runs only once on mount

  // Truncate description to a specific length
  const truncateDescription = (description, maxLength) => {
    if (description.length > maxLength) {
      return description.substring(0, maxLength) + "...";
    }
    return description;
  };

  // Handle navigation to product details page
  const handleViewMore = (id) => {
    navigate(`/product-details/${id}`);
  };

  // Handle update action (redirect to update form/page)
  const handleUpdate = (id) => {
    // Navigate to the update page with the product ID
    navigate(`/update-product/${id}`);
  };

  // Handle delete action
  const handleDelete = async (id) => {
    try {
      await axios.delete(`http://localhost:5282/api/product/delete/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      setProducts(products.filter((product) => product.id !== id)); // Remove product from state
      toast.success("Product deleted successfully.", {
        position: "top-center",
      });
    } catch (error) {
      console.error("Failed to delete product", error);
      toast.error("Failed to delete product. Please try again.", {
        position: "top-center",
      });
    }
  };

  // Custom confirmation with Toastify
  const confirmDelete = (id) => {
    toast(
      ({ closeToast }) => (
        <div>
          <p>Are you sure you want to delete this product?</p>
          <div>
            <button
              style={{
                backgroundColor: "#dc3545",
                color: "#fff",
                padding: "5px 10px",
                border: "none",
                borderRadius: "3px",
                marginRight: "5px",
                cursor: "pointer",
              }}
              onClick={() => {
                handleDelete(id);
                closeToast();
              }}
            >
              Yes, Delete
            </button>
            <button
              style={{
                backgroundColor: "#007bff",
                color: "#fff",
                padding: "5px 10px",
                border: "none",
                borderRadius: "3px",
                cursor: "pointer",
              }}
              onClick={closeToast}
            >
              Cancel
            </button>
          </div>
        </div>
      ),
      {
        position: "top-center", // Position the toast at the top center
        autoClose: false, // Disable auto-close for confirmation
        closeOnClick: false,
        closeButton: false,
      }
    );
  };

  // Handle navigation to the Add Product page
  const handleAddProduct = () => {
    navigate("/add-product");
  };

  // JSX to render the product list or a loading message
  return (
    <div
      className="container mt-5"
      style={{
        maxWidth: "900px",
        padding: "20px",
        backgroundColor: "#f9f9f9",
        borderRadius: "10px",
        boxShadow: "0 4px 10px rgba(0, 0, 0, 0.1)",
      }}
    >
      <h2 className="text-center mb-4" style={{ color: "#333" }}>
        Available Products
      </h2>

      {/* If loading, show a loading message */}
      {loading ? (
        <p className="text-center">Loading products...</p>
      ) : (
        <>
          {/* If no products available, show a message */}
          {products.length === 0 ? (
            <p className="text-center">No products available.</p>
          ) : (
            <table className="table table-bordered table-hover">
              <thead className="thead-light">
                <tr>
                  <th>Product Name</th>
                  <th>Price</th>
                  <th>Available Quantity</th>
                  <th>Category</th>
                  <th>Description</th>
                  <th className="text-center">Actions</th>
                </tr>
              </thead>
              <tbody>
                {products.map((product) => (
                  <tr key={product.id}>
                    <td>{product.productName}</td>
                    <td>${product.price.toFixed(2)}</td>
                    <td>{product.availableQuantity}</td>
                    <td>{product.category}</td>
                    <td>{truncateDescription(product.description, 50)}</td>
                    <td
                      className="text-center"
                      style={{ verticalAlign: "middle" }}
                    >
                      <div
                        style={{
                          display: "flex",
                          flexDirection: "column",
                          alignItems: "center",
                        }}
                      >
                        <button
                          style={{
                            backgroundColor: "#007bff",
                            color: "#fff",
                            padding: "8px 15px",
                            fontSize: "14px",
                            border: "none",
                            borderRadius: "5px",
                            marginBottom: "5px", // Add spacing between buttons
                            transition: "background-color 0.3s ease",
                            width: "100px", // Ensure all buttons are the same width
                            cursor: "pointer",
                          }}
                          onClick={() => handleViewMore(product.id)}
                        >
                          View More
                        </button>
                        <button
                          style={{
                            backgroundColor: "#ffc107",
                            color: "#fff",
                            padding: "8px 15px",
                            fontSize: "14px",
                            border: "none",
                            borderRadius: "5px",
                            marginBottom: "5px", // Add spacing between buttons
                            transition: "background-color 0.3s ease",
                            width: "100px", // Ensure all buttons are the same width
                            cursor: "pointer",
                          }}
                          onMouseOver={(e) =>
                            (e.currentTarget.style.backgroundColor = "#e0a800")
                          }
                          onMouseOut={(e) =>
                            (e.currentTarget.style.backgroundColor = "#ffc107")
                          }
                          onClick={() => handleUpdate(product.id)}
                        >
                          Update
                        </button>
                        <button
                          style={{
                            backgroundColor: "#dc3545",
                            color: "#fff",
                            padding: "8px 15px",
                            fontSize: "14px",
                            border: "none",
                            borderRadius: "5px",
                            transition: "background-color 0.3s ease",
                            width: "100px", // Ensure all buttons are the same width
                            cursor: "pointer",
                          }}
                          onMouseOver={(e) =>
                            (e.currentTarget.style.backgroundColor = "#c82333")
                          }
                          onMouseOut={(e) =>
                            (e.currentTarget.style.backgroundColor = "#dc3545")
                          }
                          onClick={() => confirmDelete(product.id)}
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}

          {/* Add Product Button */}
          <div className="d-flex justify-content-center mt-4">
            <button
              className="btn btn-primary"
              style={{
                backgroundColor: "#007bff",
                borderColor: "#007bff",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "5px",
              }}
              onClick={handleAddProduct}
            >
              Add Product
            </button>
          </div>
        </>
      )}

      {/* Toast Container for notifications */}
      <ToastContainer />
    </div>
  );
};

export default ProductsPage;
