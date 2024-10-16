import React, { useState, useEffect } from "react";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"; // Import toastify CSS

const OrdersPage = () => {
  const [orders, setOrders] = useState([]); // State to hold fetched orders
  const [loading, setLoading] = useState(true); // State for loading

  useEffect(() => {
    // Function to fetch orders
    const fetchOrders = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5282/api/order/all-orders",
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
        setOrders(response.data); // Set the orders from the response
        setLoading(false); // Set loading to false once orders are fetched
      } catch (error) {
        console.error("Failed to fetch orders", error);
        toast.error("Failed to fetch orders. Please try again.", {
          position: "top-right",
        });
        setLoading(false);
      }
    };

    fetchOrders(); // Call the function to fetch orders
  }, []); // Empty dependency array ensures this effect runs only once on mount

  // Truncate description to a specific length
  const truncateNote = (note, maxLength) => {
    if (note.length > maxLength) {
      return note.substring(0, maxLength) + "...";
    }
    return note;
  };

  // Function to get human-readable order status
  const getOrderStatus = (status) => {
    switch (status) {
      case 0:
        return "Pending";
      case 1:
        return "Dispatched";
      case 2:
        return "Completed";
      case 3:
        return "Delivered"; // New status for delivered orders
      default:
        return "Unknown";
    }
  };

  // Function to mark an order as delivered
  const markAsDelivered = async (orderId) => {
    try {
      // Send a request to update the order status to "Delivered"
      await axios.put(
        `http://localhost:5282/api/order/mark-delivered/${orderId}`,
        {}, // Empty body for the request
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );

      // Update the order status in the state to reflect the change
      setOrders((prevOrders) =>
        prevOrders.map((order) =>
          order.id === orderId ? { ...order, orderStatus: 3 } : order
        )
      );

      toast.success("Order marked as delivered successfully!", {
        position: "top-right",
      });
    } catch (error) {
      console.error("Failed to mark order as delivered", error);
      toast.error("Failed to mark order as delivered. Please try again.", {
        position: "top-right",
      });
    }
  };

  // JSX to render the orders list or a loading message
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
        Available Orders
      </h2>

      {/* If loading, show a loading message */}
      {loading ? (
        <p className="text-center">Loading orders...</p>
      ) : (
        <>
          {/* If no orders available, show a message */}
          {orders.length === 0 ? (
            <p className="text-center">No orders available.</p>
          ) : (
            <table className="table table-bordered table-hover">
              <thead className="thead-light">
                <tr>
                  <th>Order ID</th>
                  <th>Total</th>
                  <th>Address</th>
                  <th>Note</th>
                  <th>Status</th>
                  <th className="text-center">Actions</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id}>
                    <td>{order.id}</td>
                    <td>${order.orderTotal.toFixed(2)}</td>
                    <td>{order.deliveryAddress}</td>
                    <td>{truncateNote(order.note, 50)}</td>
                    <td>{getOrderStatus(order.orderStatus)}</td>
                    <td className="text-center">
                      {order.orderStatus !== 3 && (
                        <button
                          className="btn btn-success btn-sm" // Added Bootstrap's small button class
                          style={{
                            fontSize: "12px", // Smaller font size for a compact look
                            padding: "5px 10px", // Adjust padding to make it smaller
                            borderRadius: "4px", // Rounded corners
                            marginBottom: "4px", // Space between buttons and other content
                          }}
                          onClick={() => markAsDelivered(order.id)}
                        >
                          Mark as Delivered
                        </button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </>
      )}

      {/* Toast Container for notifications */}
      <ToastContainer />
    </div>
  );
};

export default OrdersPage;
