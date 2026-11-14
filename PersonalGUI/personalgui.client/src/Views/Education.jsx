import '../css/Edu.css';

import { useEffect } from 'react';

function Education() {

    useEffect(() => {
        document.title = "My Education";
    })

    return (
        <div>
            <h2 className="h2-title h2-title-edu-underline">Education</h2>

            <article className="edu-card">
                <div className="content-column">
                    <div className="edu-header">
                        <h3 className="edu-school">Eastern Arizona College</h3>
                        <div className="edu-date">Fall 2021 - Spring 2023</div>
                    </div>
                    
                    <div className="edu-major">Computer and Information Sciences, General</div>

                    <div className="edu-desc ti">
                        <p>During my time here, I explored programming and IT fundamentals, learning C#, Python, web development, and hands-on IT skills through TestOut-style projects. I also completed general education courses that transferred to my current university.</p>
                    </div>
                </div>
            </article>

            <div className="spacer-inbetween-schools"></div>

            <article className="edu-card">
                <div className="content-column">
                    <div className="edu-header">
                        <h3 className="edu-school">Grand Canyon University</h3>
                        <div className="edu-date">September 2023 - July 2026</div>
                    </div>
                    <div className="edu-major">Bachelor of Science in Software Development (Machine Learning Minor)</div>

                    <div className="edu-desc ti">
                        <p>
                            I have completed the core coursework for my Software Development major at Grand Canyon University, where I developed strong skills in programming, web development, and IT fundamentals. My studies included object-oriented programming, database design, algorithms, and operating systems, with a focus on hands-on projects and practical problem-solving. This program emphasized building scalable, efficient, and secure software solutions to prepare me for real-world challenges.
                        </p>

                        <p>
                            I have also finished the curriculum for my Machine Learning minor, which explored how algorithms evolve and improve with experience. The minor blended theoretical knowledge with hands-on experience in data analysis, deep learning, and ethical AI considerations.
                        </p>
                    </div>
                </div>
            </article>

            <h2 className="h2-title-extra h2-title-extra-underline">Extracurriculars</h2>

            <article className="edu-card">
                <div className="content-column">
                    <div className="edu-header">
                        <h3 className="edu-school">Canyon Artificial Intelligence Research (CAIR)</h3>
                        <div className="edu-date">Spring 2024 - Now</div>
                    </div>
                    <div className="edu-major">GCU Research Club - <a className={"edu-major"} href={"https://aigcu.org/"} target="_blank" rel="noopener noreferrer">aigcu.org</a></div>

                    <div className="edu-desc ti">
                        <p>I'm a member of CAIR, a student organization dedicated to exploring and advancing artificial intelligence research. The club provides opportunities to collaborate on AI projects, develop practical skills, and connect with peers who share a passion for innovation and problem-solving in the field of AI.</p>
                    </div>
                </div>
            </article>

            <div className="spacer-inbetween-schools"></div>

        </div>
    );
}

export default Education;