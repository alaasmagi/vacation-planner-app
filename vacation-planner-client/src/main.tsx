import { router } from "./routing/router";
import './index.css'
import { RouterProvider } from 'react-router-dom'
import ReactDOM from "react-dom/client";
import 'bootstrap/dist/css/bootstrap.min.css'

document.documentElement.setAttribute("data-bs-theme", "dark")
ReactDOM.createRoot(document.getElementById("root")!).render(
  <RouterProvider router={router} />
);
