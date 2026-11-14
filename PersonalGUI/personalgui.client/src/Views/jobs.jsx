import '../css/Jobs.css';

import { useEffect, useState } from 'react';
import { endDateCheck } from '../Services/CheckEndDate';
import { formatDate } from '../Services/FormatDate';

function Jobs() {
    const API_URL = import.meta.env.VITE_API_URL;

    const [jobs, setJobs] = useState();

    useEffect(() => {
        document.title = "My Experience";
        populateJobsData();
    }, []);

    const contents = jobs === undefined
        ? <p>Backend Is Waking Up. One Moment.</p>
        : <div className="job-container">

            {jobs.map(job =>
                <article className="job-card" key={job.dbId}>
                    <div className="content-col">
                        <h3 className="job-company">{job.company}</h3>
                        <div className="job-title">{job.positionTitle} | {formatDate(job.start)} - {endDateCheck(job.start, job.end)}</div>

                        {job.posDesc && <p className="job-desc">{job.posDesc}</p>}
                    </div>
                </article>
            )}
            
        </div>;

    return (
        <div>
            <h2 className="h2-title h2-title-job">Experience</h2>

            {contents}
        </div>
    );

    async function populateJobsData() {
        const response = await fetch(`${API_URL}api/Jobs`)
        if (response.ok) {
            const data = await response.json();
            data.sort((a, b) => a.position - b.position);
            setJobs(data);
        }
    }
}

export default Jobs;