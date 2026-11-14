import Index from './Views/index';
import Projects from './Views/projects';

import { useState } from "react";
import { NavLink } from "react-router-dom";

function Header() {
    const [isExpanded, setIsExpanded] = useState(false);

    const handleLinkClick = () => {
        setIsExpanded(false); // collapse nav when any link is clicked
    };

    return (
        <header>
            <nav className={isExpanded ? "expanded" : ""}>
                <NavLink to="/" className={({ isActive }) => isActive ? "active" : ""} onClick={handleLinkClick}>About</NavLink>
                <NavLink to="/projects" className={({ isActive }) => isActive ? "active" : ""} onClick={handleLinkClick}>Projects</NavLink>
                <NavLink to="/jobs" className={({ isActive }) => isActive ? "active" : ""} onClick={handleLinkClick}>Experience</NavLink>
                <NavLink to="/education" className={({ isActive }) => isActive ? "active" : ""} onClick={handleLinkClick}>Education</NavLink>
                <NavLink to="/resume" className={({ isActive }) => isActive ? "active" : ""} onClick={handleLinkClick}>Resumé</NavLink>
            </nav>

            <button className="nav-button" onClick={() => setIsExpanded(!isExpanded)}>
                <span className={`nav-arrow ${isExpanded ? "expanded" : ""}`}></span>
            </button>
        </header>
    );
}

export default Header;
