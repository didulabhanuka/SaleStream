import { Link, useLocation, useNavigate } from 'react-router-dom'; // Added useNavigate for redirection
import logo from '../images/logo.png';
import man from '../images/man.jpg';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { ToastContainer, toast } from 'react-toastify'; // Import Toastify for the confirmation
import 'react-toastify/dist/ReactToastify.css';

const Navbar = () => {
  const location = useLocation(); // Get the current path
  const navigate = useNavigate(); // Hook for navigation

  // Logout confirmation with toast notification
  const handleLogout = () => {
    toast(
      ({ closeToast }) => (
        <div>
          <p>Are you sure you want to logout?</p>
          <div>
            <button
              className="btn btn-danger me-2"
              onClick={() => {
                // Confirm logout action
                closeToast(); // Close the toast
                navigate('/'); // Redirect to homepage after logging out
              }}
            >
              Yes
            </button>
            <button
              className="btn btn-secondary"
              onClick={closeToast}
            >
              No
            </button>
          </div>
        </div>
      ),
      {
        position: 'top-center', // Position the confirmation toast at the top center
        autoClose: false, // Disable auto close, so the user has time to choose
      }
    );
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light shadow-sm p-2 bg-white rounded">
      <div className="container-fluid">
        <Link to="">
          <img className="mx-3" src={logo} alt='logo' width={170} height={65} />
        </Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item mx-3">
              <Link 
                to="/customer-edit" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/customer-edit' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/customer-edit' ? '#1827A4' : '' }}
              >
                Customer Activation
              </Link>
            </li>
            <li className="nav-item mx-3">
              <Link 
                to="/Add-vendor" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/Add-vendor' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/Add-vendor' ? '#1827A4' : '' }}
              >
                Add Vendor
              </Link>
            </li>
            <li className="nav-item mx-3">
              <Link 
                to="/vendor-list" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/vendor-list' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/vendor-list' ? '#1827A4' : '' }}
              >
                Vendor List
              </Link>
            </li>
            <li className="nav-item mx-3">
              <Link 
                to="/orders" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/orders' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/orders' ? '#1827A4' : '' }}
              >
                Orders
              </Link>
            </li>
            <li className="nav-item mx-3">
              <Link 
                to="/admin-products" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/admin-products' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/admin-products' ? '#1827A4' : '' }}
              >
                Products
              </Link>
            </li>
          </ul>
          <div className="d-flex align-items-center ms-auto">
            {/* Logout confirmation on clicking the image */}
            <button className="btn btn-link p-0" onClick={handleLogout}>
              <img className="mx-3" src={man} alt='man' width={35} height={35} />
            </button>
          </div>
        </div>
      </div>
      <ToastContainer /> {/* Add ToastContainer for showing notifications */}
    </nav>
  );
};

export default Navbar;