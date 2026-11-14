import '../css/AboutMe.css';

import { useEffect } from 'react';
import DasMe from '../assets/images/DasMe.jpg';

function Index() {

    useEffect(() => {
        document.title = "About Me";
    }, []);

    return (
        <div>
            <h2 className="aboutme-h2">Gavin Higgins</h2>
            <div className="aboutme">
                <div className="img-container">
                    <img className="aboutme-pic" src={DasMe} alt={"Image of Gavin the site creator"}></img>
                </div>
                
                <div className="aboutme-p ti">
                    <p>Hello, I'm Gavin, a software developer that recently graduated from GCU. My current focus is on machine learning, frontend, and backend systems. I really just like using code to build things that are actually useful in the real world.</p>

                    <p>One of my recent projects was working with a team of five to build an American Sign Language translator using computer vision. Working on it was a great way to see how AI can be used to make things more accessible, while also giving me the chance to learn how to lead a small team.</p>

                    <p>Going forward, my long-term goal is to help build AI systems and robotic assistants that do real, practical work and maybe even have a bit of personality.</p>

                    <p>In my free time, you can usually find me tinkering with Linux, modding games, hanging out with my dog, or trying out new restaurants. I like to keep things simple and value honest communication, and I'm always just looking forward to whatever I get to learn next.</p>
                </div>
            </div>

        </div>
    );
}

export default Index;