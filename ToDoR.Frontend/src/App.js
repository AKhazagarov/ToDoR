import './App.css';
import {MyTasks} from './MyTasks';
import {About} from './About';
import React, { Component } from "react";
import {BrowserRouter ,Route ,Routes ,NavLink ,Navigate} from 'react-router-dom';
import {Nav} from "react-bootstrap";
import Container from 'react-bootstrap/Container';
import Navbar from 'react-bootstrap/Navbar';

import ".//Styles/main.css";

function App() {
  return (
    <BrowserRouter>
    
      <div className="App container">
        <Navbar bg="dark" variant="dark">
          <Container>
            <Navbar.Brand  as={NavLink} to="/myTasks">ToDoR</Navbar.Brand>
            <Nav className="me-auto">
              <Nav.Link as={NavLink} to="/about">About</Nav.Link>
            </Nav>
          </Container>
        </Navbar>
        <Routes>
          <Route path='/myTasks' element={<MyTasks/>}/>
          <Route path='/about' element={<About/>}/>
          <Route path="/" element={<Navigate replace to="/myTasks" />} />
        </Routes>        
      </div>
    </BrowserRouter>
  );
}

export default App;