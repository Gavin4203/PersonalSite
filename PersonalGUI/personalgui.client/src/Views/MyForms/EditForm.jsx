import '../../css/MyForms/Forms.css';

import { useEffect, useState } from 'react';
import { sanitizeInput } from '../../Security/SanitizeInput';

function Edit() {
    const API_URL = import.meta.env.VITE_API_URL;

    const [projects, setProjects] = useState();
    const [jobs, setJobs] = useState();

    const [selectedData, setSelectedData] = useState("none");
    const [selectedType, setSelectedType] = useState("none");
    
    const [maxP, setMaxP] = useState(-1);
    const [maxJ, setMaxJ] = useState(-1);

    useEffect(() => {
        populateData();
    }, []);

    function setSelectedItem(selected, type) {
        setSelectedData(selected);
        setSelectedType(type);
    }

    return (
        <div>
            <h1>Edit Form</h1>

            {getForm(selectedData,selectedType)}

            {selectionBox()}

        </div>
    );

    //Data Population-----------------------------------------------------------------------------------------------------------------------------------------------------------

    async function populateData() {

        try {
            const [projRes, jobRes] = await Promise.all([
                fetch(`${API_URL}api/Edit/getP`, {
                    credentials: "include"
                }),
                fetch(`${API_URL}api/Edit/getJ`, {
                    credentials: "include"
                })
            ]);

            if (projRes.ok) {
                const dataP = await projRes.json();
                dataP.sort((a,b) => a.position - b.position);
                const maxPP = Math.max(...dataP.map((dp) => dp.position));
                setProjects(dataP);
                setMaxP(maxPP);
            }

            if (jobRes.ok) {
                const dataJ = await jobRes.json();
                dataJ.sort((a,b) => a.position - b.position);
                const maxJP = Math.max(...dataJ.map((dj) => dj.position));
                setJobs(dataJ);
                setMaxJ(maxJP);
            }
        } catch (err) {
            console.error('Failed to fetch data', err)
        }
    }

    //Selection Box--------------------------------------------------------------------------------------------------------------------------------------------------

    function selectionBox() {
        const contents = projects == undefined
            ? <p>Can't Find Anything. Try Refreshing.</p>
            : <form>
                <h2>Projects</h2>
                {projects.map(project =>
                    <label key={`proj-${project.dbId}`}>
                        {project.title}
                        <input type="radio" name="editRadioBtn" value={`proj-${project.dbId}`} onChange={() => setSelectedItem(project, "proj")} />
                        <br></br>
                    </label>
                )}
                <h2>Jobs</h2>
                {jobs.map(job =>
                    <label key={`job-${job.dbId}`}>
                        {job.company} - {job.positionTitle}
                        <input type="radio" name="editRadioBtn" value={`job-${job.dbId}`} onChange={() => setSelectedItem(job, "job")} />
                        <br></br>
                    </label>
                )}
            </form>;

        return contents;
    }

    //Submissions----------------------------------------------------------------------------------------------------------------------------------------------------

    async function handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData(e.target);

        const action = e.nativeEvent.submitter.value;

        if (action === "update-proj") {
            console.log("Project Update Button Clicked");

            await updateProj(formData);

        } else if (action === "update-job") {
            console.log("Job Update Button Clicked");

            await updateJob(formData);

        } else if (action === "delete-proj") {
            console.log("Project Delete Button Clicked");

            await delProj();

        } else if (action === "delete-job") {
            console.log("Job Delete Button Clicked");

            await delJob();

        }
    }

    async function updateProj(formData) {

        const data = {
            dbId: selectedData.dbId,
            title: sanitizeInput(formData.get("ProjTitle")),
            projDesc: sanitizeInput(formData.get("ProjDesc")),
            techStack: sanitizeInput(formData.get("TechStack")),
            startDate: formData.get("start"),
            endDate: formData.get("end") || null,
            icon: sanitizeInput(formData.get("icon")),
            position: formData.get("position")
        };

        const res = await fetch(`${API_URL}api/Edit/updateP`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify(data)
        });

        if (res.ok) {
            const updatedModel = await res.json();
            setProjects(prev => prev.map(p => p.dbId === updatedModel.dbId ? updatedModel : p));
        }

    }

    async function updateJob(formData) {

        const data = {
            dbId: selectedData.dbId,
            company: sanitizeInput(formData.get("company")),
            positionTitle: sanitizeInput(formData.get("jobTitle")),
            posDesc: sanitizeInput(formData.get("jobDesc")),
            start: formData.get("start"),
            end: formData.get("end") || null,
            postion: formData.get("position")
        };

        const res = await fetch(`${API_URL}api/Edit/updateJ`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify(data)
        });

        if (res.ok) {
            const updatedModel = await res.json();
            setJobs(prev => prev.map(p => p.dbId === updatedModel.dbId ? updatedModel : p));
        }

    }

    async function delProj() {

        const res = await fetch(`${API_URL}api/Edit/deleteP`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify(selectedData.dbId)
        });

        if (res.ok) {
            const id = await res.json();
            setProjects(prev => prev.filter(proj => proj.dbId !== id));
        }
    }

    async function delJob() {

        const res = await fetch(`${API_URL}api/Edit/deleteJ`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify(selectedData.dbId)
        });

        if (res.ok) {
            const id = await res.json();
            setJobs(prev => prev.filter(job => job.dbId !== id));
        }
    }

    //Forms----------------------------------------------------------------------------------------------------------------------------------------------------------

    function getForm(data,type) {
        if (type === "none") {
            return (noTypeForm());
        }

        if (type === "proj") {
            return (projForm(data));
        }

        if (type === "job") {
            return (jobForm(data));
        }
    }

    function noTypeForm() {
        return (
            <div>
                <h3>Nothing Selected</h3>
            </div>
        );
    }

    function projForm(data) {
        const contents = data === undefined
            ? <h3>Failed To Get Data!</h3>
            : <form key={`proj-${data.dbId}`} onSubmit={handleSubmit}>
                <h2>Edit Project</h2>
                <label>
                    Project Title
                    <input name="ProjTitle" type="text" defaultValue={data.title} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Project Description
                    <textarea className="large-input" name="ProjDesc" type="text" defaultValue={data.projDesc} maxLength="250" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Tech Stack
                    <textarea className="med-input" name="TechStack" type="text" defaultValue={toString(data.techStack)} maxLength="50" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Project Start Date
                    <input name="start" type="date" defaultValue={data.startDate} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Project End Date
                    <input name="end" type="date" defaultValue={data.endDate} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Icon
                    <select name="icon" defaultValue={data.icon}>
                        <option value="appWindow">App Window</option>
                        <option value="code">Code</option>
                        <option value="gameController">Game Controller</option>
                        <option value="headCircuit">Head Circuit</option>
                        <option value="webProj">Web Project</option>
                    </select>
                </label>
                <br></br>
                <br></br>
                <label>
                    Position
                    <input name="position" type="number" min="0" max={maxP} defaultValue={data.position}
                           onKeyDown={(e) => e.preventDefault()}
                           onPaste={(e) => e.preventDefault()}
                    />
                </label>
                <br></br>
                <br></br>
                <button className="input-button" type="submit" name="action" value="update-proj">Update</button>
                <br></br>
                <br></br>
                <button className="input-button" type="submit" name="action" value="delete-proj">Delete</button>

            </form>

        return contents;
    }

    function jobForm(data) {
        const contents = data === undefined
            ? <h3>Failed To Get Data!</h3>
            : <form key={`job-${data.dbId}`} onSubmit={handleSubmit}>
                <h2>Edit Job</h2>
                <label>
                    Company Name
                    <input name="company" type="text" defaultValue={data.company} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job Title
                    <input name="jobTitle" type="text" defaultValue={data.positionTitle} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job Description
                    <textarea className="large-input" name="jobDesc" type="text" defaultValue={data.posDesc} maxLength="300" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job Start Date
                    <input name="start" type="date" defaultValue={data.start} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job End Date
                    <input name="end" type="date" defaultValue={data.end} />
                </label>
                <br></br>
                <br></br>
                <label>
                    Position
                    <input name="position" type="number" min="0" max={maxJ} defaultValue={data.position} 
                           onKeyDown={(e) => e.preventDefault()}
                           onPaste={(e) => e.preventDefault()}
                    />
                </label>
                <br></br>
                <br></br>
                <button className="input-button" type="submit" name="action" value="update-job">Update</button>
                <br></br>
                <br></br>
                <button className="input-button" type="submit" name="action" value="delete-job">Delete</button>

            </form>

        return contents;
    }

    //Formatting-----------------------------------------------------------------------------------------------------------------------------------------------------

    function toString(arr) {
        let csv = "";
        let size = arr.length;
        let idx = 1;
        for (let item of arr) {
            if(idx === size){
                csv += `${item}`;
            }
            else{
                csv += `${item},`;
                idx += 1;
            }
        }

        return csv;
    }
}

export default Edit;