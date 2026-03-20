import { createBrowserRouter } from "react-router-dom";
import Home from "../views/Home";
import Details from "../views/Details";
import Edit from "../views/Edit";

export const router = createBrowserRouter([
  { path: "/", element: <Home /> },
  { path: "/edit/new", element: <Edit /> },
  { path: "/edit/:id", element: <Edit /> },
  { path: "/details/:id", element: <Details /> },
]);