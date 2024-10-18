import logo from './logo.svg';
import React from "react";
import { Routes, Route, BrowserRouter } from "react-router-dom";
import Register from "./Admin/user/Register";
//import { User } from "./Admin/user/User";
//import { UpdateUser } from "./Admin/user/UpdateUser";
//import { CSRUsers } from "./Admin/user/CSRUsers/CSRUsers";
//import { Product } from "./Admin/Product/Products";
//import { AddProduct } from "./Admin/Product/AddProduct"; 
//import { UpdateProduct } from "./Admin/Product/UpdateProduct";
import UserSelection from "./Admin/UserSelection" 
import Login from "./Admin/user/Login"
import CustomerEdit from "./Admin/CustomerEdit"
import AddVendor from "./Admin/AddVendor"
import VendorList from "./Admin/VendorList"
import VendorComment from "./Admin/VendorComment";
import VendorLogin from "./Admin/user/VendorLogin"
import CsrCustomerEdit from "./Admin/CsrCustomerEdit"

import AddProduct from "./Admin/Products/AddProduct";
import UpdateProduct from "./Admin/Products/UpdateProduct";
import ProductsPage from "./Admin/Products/Products";
import CsrProductsPage from "./Admin/Products/CsrProducts";
import ProductDetails from "./Admin/Products/ProductDetails";
import OrdersPage from "./Admin/Orders/OrdersPage";
import AdminProducts from "./Admin/Products/AdminProducts";
import VendorOrdersPage from "./Admin/Orders/VendorOrdersPage";
import CsrOrdersPage from "./Admin/Orders/CsrOrdersPage";

function App() {
  return (
    <div>
      <BrowserRouter>
        <Routes>
          {/* Admin routes */}
         <Route exact path="/reg" element={<Register />} />
          {/* <Route exact path="/user" element={<User />} />
          <Route exact path="/csr-users" element={<CSRUsers />} />
          <Route path="/update-user/:id" element={<UpdateUser />} />
          <Route exact path="/products" element={<Product />} />
          <Route exact path="/add-product" element={<AddProduct />} />{" "}
          <Route exact path="/update-product/:id" element={<UpdateProduct />} /> */}
          {/* AddProduct route */}
          {/* Add more routes here */}

          <Route exact path="/" element={<UserSelection />} />
          <Route exact path="/log" element={<Login />} />
          <Route exact path="/customer-edit" element={<CustomerEdit />} />
          <Route exact path="/add-vendor" element={<AddVendor />} />
          <Route exact path="/vendor-list" element={<VendorList />} />
          <Route path="/vendor-comment/:vendorId" element={<VendorComment />} />
          <Route exact path="/vendor-login" element={<VendorLogin />} />
          <Route exact path="/csr-customer-edit" element={<CsrCustomerEdit />} />

          <Route path="/products" element={<ProductsPage />} />
          <Route path="/csr-products" element={<CsrProductsPage />} />
          <Route exact path="/add-product" element={<AddProduct />} />
          <Route exact path="/update-product/:id" element={<UpdateProduct />} />
          <Route path="/product-details/:id" element={<ProductDetails />} />
          <Route path="/orders" element={<OrdersPage />} />
          <Route path="/admin-products" element={<AdminProducts />} />
          <Route path="/vendor-orders" element={<VendorOrdersPage />} />
          <Route path="/csr-orders" element={<CsrOrdersPage />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
