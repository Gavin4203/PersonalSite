import '../css/ProjectsCard.css';

import AppWindow from '../assets/icons/app-window.svg';
import CodeSvg from '../assets/icons/code.svg';
import GameController from '../assets/icons/game-controller.svg';
import HeadCircuit from '../assets/icons/head-circuit.svg';
import WebProj from '../assets/icons/web-project.svg';

import { useEffect, useState } from 'react';
import { endDateCheck } from '../Services/CheckEndDate';
import { formatDate } from '../Services/FormatDate';

function Projects() {
    const API_URL = import.meta.env.VITE_API_URL;

    const [projects, setProjects] = useState();

    useEffect(() => {
        document.title = "My Projects";
        populateProjectData();
    }, []);

    const iconMap = {
        appWindow: AppWindow,
        code: CodeSvg,
        gameController: GameController,
        headCircuit: HeadCircuit,
        webProj: WebProj
    }

    const contents = projects === undefined
        ? <p>Backend Is Waking Up. One Moment.</p>
        : <div className="projects-grid">
            {projects.map(project =>
                <article className={`project-card`} key={project.dbId}>
                    <div className="card-header">
                        <div className="card-logo">
                            <img src={iconMap[project.icon]}></img>
                        </div>

                        <div className="card-title-wrap">
                            <h3 className="project-title">{project.title}</h3>
                            <div className="project-sub-date">{formatDate(project.startDate)} - {endDateCheck(project.startDate, project.endDate)}</div>
                        </div>

                    </div>

                    <p className="project-desc">{project.projDesc}</p>

                    <div className="tech-list">
                        {project.techStack.join(", ")}
                    </div>

                </article>)}
        </div>;

    return (
        <div>
            <h2 className="h2-title h2-title-proj">Projects</h2>
            

            {contents}

        </div>
    );

    async function populateProjectData() {
        const response = await fetch(`${API_URL}api/Projects`);
        if (response.ok) {
            const data = await response.json();
            data.sort((a, b) => a.position - b.position);
            setProjects(data);
        }
    }

}

export default Projects;