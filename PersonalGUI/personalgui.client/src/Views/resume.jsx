import { useEffect } from 'react';

function Resume() {
    const API_URL = import.meta.env.VITE_API_URL;

    useEffect(() => {
        document.title = "My Resume";
    });

    return (
        <iframe
            src={`${API_URL}api/Resumay#navpanes=0`}
            className="iframe-resume"
            title="Resume"
        />
    );
}

export default Resume;