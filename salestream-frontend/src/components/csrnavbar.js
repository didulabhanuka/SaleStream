import { Link, useLocation, useNavigate } from 'react-router-dom';
import logo from '../images/logo.png';
import man from '../images/man.jpg';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { ToastContainer, toast } from 'react-toastify'; // Import Toastify
import 'react-toastify/dist/ReactToastify.css'; // Import Toastify CSS

const Navbar = () => {
  const location = useLocation(); // Get the current path
  const navigate = useNavigate(); // Hook for navigation

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
                closeToast();
                navigate('/'); // Redirect to login page after logging out
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
        position: 'top-center', // Use string instead of constant for position
        autoClose: false, // Don't auto-close the toast, wait for user interaction
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
                to="/csr-customer-edit" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/csr-customer-edit' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/csr-customer-edit' ? '#1827A4' : '' }}
              >
                Account Activation
              </Link>
            </li>
            <li className="nav-item mx-3">
              <Link 
                to="/csr-orders" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/csr-orders' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/csr-orders' ? '#1827A4' : '' }}
              >
                Orders 
              </Link>
            </li>
            <li className="nav-item mx-3">
              <Link 
                to="/csr-products" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/csr-products' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/csr-products' ? '#1827A4' : '' }}
              >
                Products 
              </Link>
            </li>
          </ul>
          <div className="d-flex align-items-center ms-auto">
            <button className="btn btn-link p-0" onClick={handleLogout}>
              <img className="mx-3" src={man} alt='man' width={35} height={35} />
            </button>
          </div>
        </div>
      </div>
      <ToastContainer /> {/* Add ToastContainer for showing notifications */}
    </nav>
  );
}

export default Navbar;
