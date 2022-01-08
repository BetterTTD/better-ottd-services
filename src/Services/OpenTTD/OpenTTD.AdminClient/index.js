import Storage from "./storage.js";
import { default as Startup } from "./startup.js";

// Initialize

const storage = new Storage([]);
const services = Startup.ConfigureServices(storage);
const app = Startup.ConfigureApplication(services);

// Start

services.Hub.start().catch(err => console.log(err));

const port = process.env.DOCKER ? 80 : 6001;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});