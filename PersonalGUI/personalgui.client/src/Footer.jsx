import { useState } from 'react';

import "./css/Footer.css";

import GitHubLogo from './assets/CmpyIcons/github-mark-white.svg';
import LinkedInLogo from './assets/CmpyIcons/LI-In-Bug.png';
import EmailIcon from './assets/icons/footer/envelope-fill.svg';
import PhoneIcon from './assets/icons/footer/phone-duotone.svg';

function Footer() {

    //Quick Change Contact Info
    const phone = '928-432-2727';
    const email = 'GavinHiggins22@proton.me'
    
    const [tooltip, setTooltip] = useState({ phone: phone, email: email });

    const handleCopy = async (type) => {
        const textToCopy = type === 'phone' ? phone : email;
        await navigator.clipboard.writeText(textToCopy);
        setTooltip((prev) => ({ ...prev, [type]: 'Copied!' }));
        setTimeout(() => {
            setTooltip((prev) => ({
                ...prev,
                [type]: textToCopy
            }));
        }, 1500);
    };

    return (
        <footer id="footer">
            <div className="madeby">
                Made By: Gavin Higgins
            </div>

            <div className="contact">

                <div className="contact-icon">
                    <img id="phone-num" src={PhoneIcon} alt="Phone" onClick={() => handleCopy('phone')} />
                    <span className="tooltip">
                        {tooltip.phone}
                    </span>
                </div>
                

                <div className="div-spacer"></div>

                <div className="contact-icon">
                    <img id="email-address" src={EmailIcon} alt="Email" onClick={() => handleCopy('email')} />
                    <span className="tooltip">
                        {tooltip.email}
                    </span>
                </div>
                

                <div className="div-spacer"></div>

                <a className="contact-icon" href="https://github.com/Gavin4203" target="_blank" rel="noopener noreferrer">
                    <img src={GitHubLogo} />
                    <span className="tooltip ttgithub">
                        https://github.com/Gavin4203
                    </span>
                </a>

                <div className="div-spacer"></div>

                <a className="contact-icon" href="https://www.linkedin.com/in/gavin-higgins-74774a210" target="_blank" rel="noopener noreferrer">
                    <img src={LinkedInLogo}></img>
                    <span className="tooltip ttlinkedin">
                        https://www.linkedin.com/in/gavin-higgins-74774a210
                    </span>
                </a>
            </div>
        </footer>
    );
}

export default Footer;