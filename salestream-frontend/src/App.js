import logo from "./logo.svg";
import React from "react";
import { Routes, Route, BrowserRouter } from "react-router-dom";
//import Register from "./Admin/user/Register";
//import { User } from "./Admin/user/User";
//import { UpdateUser } from "./Admin/user/UpdateUser";
//import { CSRUsers } from "./Admin/user/CSRUsers/CSRUsers";
//import { Product } from "./Admin/Product/Products";
//import { AddProduct } from "./Admin/Product/AddProduct";
import Login from "./Admin/user/Login";
import CustomerEdit from "./Admin/CustomerEdit";
import AddProduct from "./Admin/Products/AddProduct";
import UpdateProduct from "./Admin/Products/UpdateProduct";
import ProductsPage from "./Admin/Products/Products";
import ProductDetails from "./Admin/Products/ProductDetails";
import OrdersPage from "./Admin/Orders/OrdersPage";

function App() {
  return (
    <div>
      <BrowserRouter>
        <Routes>
          {/* Admin routes */}
          {/* <Route exact path="/reg" element={<Register />} />
          <Route exact path="/user" element={<User />} />
          <Route exact path="/csr-users" element={<CSRUsers />} />
          <Route path="/update-user/:id" element={<UpdateUser />} />
          <Route exact path="/products" element={<Product />} />
          <Route exact path="/add-product" element={<AddProduct />} />{" "}
           */}
          <Route exact path="/log" element={<Login />} />
          <Route exact path="/customer-edit" element={<CustomerEdit />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route exact path="/add-product" element={<AddProduct />} />
          <Route exact path="/update-product/:id" element={<UpdateProduct />} />
          <Route path="/product-details/:id" element={<ProductDetails />} />
          <Route path="/orders" element={<OrdersPage />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
