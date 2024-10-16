import { Link, useLocation } from 'react-router-dom';
import logo from '../images/logo.png';
import man from '../images/man.jpg';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';


const Navbar = () => {
  const location = useLocation(); // Get the current path

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light shadow-sm p-2 bg-white rounded">
      <div className="container-fluid">
        <Link to="/">
          <img className="mx-3" src={logo} alt='logo' width={170} height={65} />
        </Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item mx-3">
              <Link 
                to="/home" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/home' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/home' ? '#1827A4' : '' }}
              >
                Home
              </Link>
            </li>
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
                to="/vendor-list" 
                className={`nav-link fs-5 fw-bold ${location.pathname === '/vendor-list' ? 'text-primary' : ''}`}
                style={{ color: location.pathname === '/vendor-list' ? '#1827A4' : '' }}
              >
                Products
              </Link>
            </li>
          </ul>
          <div className="d-flex align-items-center ms-auto">
        
            <Link to="/log">
              <img className="mx-3" src={man} alt='man' width={35} height={35} />
            </Link>
          </div>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;