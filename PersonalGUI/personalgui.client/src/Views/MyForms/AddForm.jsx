import '../../css/MyForms/Forms.css';

import { useState } from 'react';
import { sanitizeInput } from "../../Security/SanitizeInput";

function Add() {
    const API_URL = import.meta.env.VITE_API_URL;

    const [selectedType, setSelectedType] = useState("proj");

    function setPressedItem(type) {
        setSelectedType(type);
    }

    return (
        <div>
            <h1>Add Form</h1>

            {selectionBox()}

            {getForm(selectedType)}

        </div>
    );

    //Selection Box--------------------------------------------------------------------------------------------------------------------------------------------------

    function selectionBox() {
        return (
            <div>
                <button className="input-button" type="button" name="projBtn" value="proj" onClick={() => setPressedItem("proj")}>Project</button>

                <button className="input-button" type="button" name="jobBtn" value="job" onClick={() => setPressedItem("job")}>Job</button>

                <button className="input-button" type="button" name="resumeBtn" value="resume" onClick={() => setPressedItem("resume")}>Resume</button>
            </div>
        );
    }

    //Submissions----------------------------------------------------------------------------------------------------------------------------------------------------

    async function handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData(e.target);

        const action = e.nativeEvent.submitter.value;

        console.log("Action");
        console.log(`${action}`);

        if (action === "add-proj") {
            console.log("Project Add Button Clicked");

            await addProj(formData);

        } else if (action === "add-job") {
            console.log("Job Add Button Clicked");

            await addJob(formData);

        } else if (action === "add-resume") {
            console.log("Resume Add Button Clicked");

            await addResume(formData);
        }
    }

    async function addProj(formData) {

        const data = {
            dbId: -1,
            title: sanitizeInput(formData.get("ProjTitle")),
            projDesc: sanitizeInput(formData.get("ProjDesc")),
            techStack: sanitizeInput(formData.get("TechStack")),
            startDate: formData.get("start"),
            endDate: formData.get("end") || null,
            icon: sanitizeInput(formData.get("icon")),
            position: -1
        };

        const res = await fetch(`${API_URL}api/Add/addP`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify(data)
        });

        if (!res.ok) {
            console.log("Problem Adding Project");
        }

    }

    async function addJob(formData) {

        const data = {
            dbId: -1,
            company: sanitizeInput(formData.get("company")),
            positionTitle: sanitizeInput(formData.get("jobTitle")),
            posDesc: sanitizeInput(formData.get("jobDesc")),
            start: formData.get("start"),
            end: formData.get("end") || null,
            position: -1
        };

        const res = await fetch(`${API_URL}api/Add/addJ`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify(data)
        });

        if (!res.ok) {
            console.log("Problem Adding Job");
        }

    }

    async function addResume(formData) {

        const res = await fetch(`${API_URL}api/Add/resumeUpload`, {
            method: 'POST',
            credentials: "include",
            body: formData
        })

        if (!res.ok) {
            console.log("Problem Uploading Resume");
        }
    }

    //Forms----------------------------------------------------------------------------------------------------------------------------------------------------------

    function getForm(type) {
        if (type === "proj") {
            return (projForm());
        }

        if (type === "job") {
            return (jobForm());
        }

        if (type === "resume") {
            return (resumeForm());
        }
    }

    function resumeForm() {
        return (
            <form key="resume-add" onSubmit={handleSubmit}>
                <h2>Resume Upload</h2>
                <input name="resUpload" type="file" accept="application/pdf" onChange={handleFileChange} />
                <br></br>
                <button className="input-button" type="submit" name="action" value="add-resume">Add</button>
            </form>
        );
    }

    function projForm() {
        return (
            <form key={`proj-add`} onSubmit={handleSubmit}>
                <h2>Add Project</h2>
                <label>
                    Project Title
                    <input name="ProjTitle" type="text" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Project Description
                    <textarea className="large-input" name="ProjDesc" type="text" maxLength="250" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Tech Stack
                    <textarea className="med-input" name="TechStack" type="text" maxLength="50" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Project Start Date
                    <input name="start" type="date" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Project End Date
                    <input name="end" type="date" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Icon
                    <select name="icon" defaultValue="">
                        <option value="" disabled>-- Select an icon --</option>
                        <option value="appWindow">App Window</option>
                        <option value="code">Code</option>
                        <option value="gameController">Game Controller</option>
                        <option value="headCircuit">Head Circuit</option>
                        <option value="webProj">Web Project</option>
                    </select>
                </label>
                <br></br>
                <br></br>
                <button className="input-button" type="submit" name="action" value="add-proj">Add</button>

            </form>
        );
    }

    function jobForm() {
        return (
            <form key={`job-add`} onSubmit={handleSubmit}>
                <h2>Add Job</h2>
                <label>
                    Company Name
                    <input name="company" type="text" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job Title
                    <input name="jobTitle" type="text" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job Description
                    <textarea className="large-input" name="jobDesc" type="text" maxLength="300" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job Start Date
                    <input name="start" type="date" />
                </label>
                <br></br>
                <br></br>
                <label>
                    Job End Date
                    <input name="end" type="date" />
                </label>
                <br></br>
                <br></br>
                <button className="input-button" type="submit" name="action" value="add-job">Add</button>

            </form>
        );
    }

    //Security-----------------------------------------------------------------------------------------------------------------------------------------------------
    function handleFileChange(e) {
        const file = e.target.files[0];
        if (file && file.type !== "application/pdf") {
            alert("Please select a pdf file only");
            e.target.value = "";
        }
    }
}

export default Add;