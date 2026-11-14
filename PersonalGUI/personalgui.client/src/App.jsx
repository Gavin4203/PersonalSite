import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import './css/index.css';

import Header from './Header';
import Footer from './Footer';
import Index from './Views/index';
import Projects from './Views/projects';
import Jobs from './Views/jobs';
import Education from './Views/Education';
import Resume from './Views/resume';
import Admin from './Views/MyForms/Admin';
import Edit from './Views/MyForms/EditForm';
import Add from './Views/MyForms/AddForm';

function App() {

    return (
        <div className="app-wrapper">
            <Router>
                <Header />

                <main className="app-main">
                    <Routes>
                        <Route path="/" element={<Index />} />
                        <Route path="/projects" element={<Projects />} />
                        <Route path="/jobs" element={<Jobs />} />
                        <Route path="/education" element={<Education />} />
                        <Route path="/resume" element={<Resume />} />
                        <Route path="/admin" element={<Admin />} />
                        <Route path="/edit" element={<Edit />} />
                        <Route path="/add" element={<Add />} />
                    </Routes>
                </main>

            </Router>

            <Footer />
        </div>
    );
}

export default App;