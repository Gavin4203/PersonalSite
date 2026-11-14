function Admin() {

    async function handleSubmit(e) {
        const API_URL = import.meta.env.VITE_API_URL;

        //prevent the browser from refreshing the page
        e.preventDefault();

        //Read form data
        const form = e.target;
        const formData = new FormData(form);

        const result = await fetch(`${API_URL}api/Admin/pass`, {
            method: "POST",
            credentials: "include",
            body: formData
        });

        if (result.ok) {
            console.log("Correct Password");
        }
        else {
            console.log("Wrong Password");
        }
    }

    return (
        <div>
            <h1>Password</h1>

            <form onSubmit={handleSubmit}>
                <label>
                    Password: <input name="password" type="password" defaultValue="Password123" />
                </label>
                <button className="input-button" type="submit">Submit</button>
            </form>
        </div>
    );

}

export default Admin;