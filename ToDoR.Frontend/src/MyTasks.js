import React, { Component, useState } from 'react';
import { variables } from './Variables.js';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import DatePicker from "react-datepicker";
import Collapse from "react-bootstrap/Collapse";
import Moment from 'moment';
import "react-datepicker/dist/react-datepicker.css";
import "bootstrap/dist/css/bootstrap.css";
import ".//Styles/main.css";

const today = new Date();

function parseDate(today){
  if (today === null){
    return "";
  }
  const dateSrc = today.toLocaleString('ru-RU', { year: 'numeric', month: 'numeric', day: 'numeric' });    
  return dateSrc.split(".").reverse().join("-");
}

export class MyTasks extends Component {

  constructor(props) {
    super(props);

    this.state = {
      myTasks: [],
      myCompletedTasks: [],
      id: "",
      userId: "00000000-0000-0000-0000-000000000000",
      name: "",
      nameAdd: "",
      note: "",
      noteAdd: "",
      dueDate: new Date(),
      dueDateAdd: new Date(),
      status: 1,
      collapseMenu: true,
      show: false
    }
    this.showHide = this.showHide.bind(this);
  }
  showHide(e) {
    e.preventDefault();

    this.setState({
      collapseMenu: !this.state.collapseMenu
    });
  }

  refreshList() {
    fetch(variables.API_URL + 'Tasks/byFilter', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        status: [0]
      })
    })
    .then(response => response.json())
    .then(data => {
      this.setState({ myTasks: data });
    });       

    fetch(variables.API_URL + 'Tasks/byFilter', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        status: [1]
      })
    })
    .then(response => response.json())
    .then(data => {
      this.setState({ myCompletedTasks: data });
    });
  }
  
  componentDidMount() {
    this.refreshList();
  }
  
  changeTaskNameForAdd = (e) => {
    this.setState({ nameAdd: e.target.value });
  }
  changeNoteForAdd = (e) => {
    this.setState({ noteAdd: e.target.value });
  }
  changeDueDateForAdd = (e) => {
    this.setState({ dueDateAdd: e});
  }

  changeTaskNameForUpdate = (e) => {
    this.setState({ name: e.target.value });
  }
  changeNoteForUpdate = (e) => {
    this.setState({ note: e.target.value });
  }
  changeDueDateForUpdate = (e) => {
    this.setState({ dueDate: e});
  }

  editClick(dep) {
    function getDate(task) {
      if (task?.dueDate) {
      return new Date(dep.dueDate);
      }
      return null;
    }
    this.toogleModal(true);
    this.setState({
      id: dep.id,
      name: dep.name,
      note: dep.note,
      dueDate: getDate(dep),
      repeat: dep.repeat,
    });
  }

  async createClick() {
    try {
      const response = await fetch(variables.API_URL + 'Tasks', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: this.state.nameAdd,
          note: this.state.noteAdd,
          dueDate: this.state.dueDateAdd,
        })
      });
      const status = await response.status;

      if (status === 400) {
        throw await response.json();
      }

      if (status) {
        const status = await this.refreshList();

      }
    } catch (error) {
      console.error(JSON.stringify(error));
      alert(error);

    } finally {
      this.toogleModal(false);
    }
  }

  async updateClick() {
    try {
      const response = await fetch(variables.API_URL + 'Tasks', {
        method: 'PUT',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          id: this.state.id,
          name: this.state.name,
          note: this.state.note,
          dueDate: this.state.dueDate,
        })
      });
      const status = await response.status;

      if (status === 400) {
        throw await response.json();
      }

      if (status) {
        const status = await this.refreshList();

      }
    } catch (error) {
      console.error(JSON.stringify(error));
      alert(error);

    } finally {
      this.toogleModal(false);
    }
  }

  async deleteClick(id) {
    if (window.confirm('Are you sure?')) {
      try {
        const response = await fetch(variables.API_URL + 'Tasks/' + id, {
          method: 'DELETE',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          }
        });
        const result = await response.status;
        if (result) {
          const status = await this.refreshList();
        }
      } catch (error) {
        console.error(error);
        alert('Failed');
      }
    }
  }
  
  async changeStatusClick(task) {
    try {
      var response;
      if (task?.status === 0){
        response = await fetch (variables.API_URL + 'Tasks/complete/' + task.id, {
          method: 'PUT',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          }
        });
      }
      else if (task?.status === 1){
        response = await fetch (variables.API_URL + 'Tasks/notComplete/' + task.id, {
          method: 'PUT',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          }
        });
      }
      const result = await response.status;
      if (result) {
        const status = await this.refreshList();
      }
    } catch (error) {
      console.error(error);
      alert('Failed');
    }
  }

  toogleModal(show) {
    this.setState({
      show
    })
  }

  taskCallback(dep) {
    function getRowClassName(task) {
      if (task?.dueDate) {
        const dueDate = new Date(task.dueDate).getTime();
        const now = new Date().getTime();
        if (task.status != 1 && dueDate < now.Date) {
          return "table-danger";
        }
      }
      return "";
    }
    function getDoneSvg(task) {
      if (task?.status != 1) {
        return "M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z";
      }
      return "M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z";
    }
    
    function getDate(task) {
      if (task?.dueDate) {
      return Moment(dep.dueDate).format('DD.MM.YYYY');
      }
      return "Not set";
    }
    
    const className = getRowClassName(dep);
    const pathSvf = getDoneSvg(dep);
    return (
      <tr className={className} key={dep.id}>
        <td>
          <button type="button"
            className="btn btn-link"
            onClick={() => this.changeStatusClick(dep)}>
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-circle" viewBox="0 0 16 16">
              <path d={pathSvf}/>
            </svg>
          </button>
        </td>
        <td>{dep.name}</td>
        <td>{dep.note}</td>
        <td>{getDate(dep)}</td>
        <td>
          <button type="button"
            className="btn btn btn-link mr-1"
            onClick={() => this.editClick(dep)}>
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
              <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
              <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
            </svg>
          </button>
          <button type="button"
            className="btn btn btn-link mr-1"
            onClick={() => this.deleteClick(dep.id)}>
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
              <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
            </svg>
          </button>

        </td>
      </tr>
    )
  }

  render() {
    const {
      myTasks,
      myCompletedTasks,
      name,
      nameAdd,
      note,
      noteAdd,
      dueDate,
      dueDateAdd,
    } = this.state;

    return (
      <div>
        <table className="table table-hover">
          <tbody>
          <tr className="table-info">
            <td colSpan="6">
              <form>
                <div className="row">
                  <div className="col-md-6">
                  <Form.Control value={nameAdd} onChange={this.changeTaskNameForAdd} type="text" placeholder="Meine Aufgabe ist..." />
                  </div>
                  <div className="col-md-3">
                  <Form.Control value={noteAdd} onChange={this.changeNoteForAdd} type="text" placeholder="Sie können eine Beschreibung hinzufügen..." />
                  </div>
                  <div className="col-md-2">
                    <DatePicker
                      selected={dueDateAdd}
                      dateFormat="dd.MM.yyyy"
                      onChange={this.changeDueDateForAdd}
                      className="form-control"
                      minDate={today}
                    />
                  </div>                  
                  <div className="col-md-1">
                    <Button variant="primary" type="submit" onClick={() => this.createClick()}>Hinzufügen</Button>
                  </div>
                </div>
              </form>
            </td>
          </tr>
          <tr>            
            <th></th>
            <th>To Do</th>
            <th>Note</th>
            <th>Due Date</th>
            <th></th>
          </tr>
            {myTasks.map(this.taskCallback, this)}
          </tbody>
        </table>
        
        <div className="accordion">        
          <div className="card">
              <div className="card-header" id="headingOne">
                <h5 className="mb-0">
                  <button className="btn btn-link" onClick={this.showHide}>
                    Show completed
                  </button>
                </h5>
              </div>
              <div id="collapseOne" className="collapse show" aria-labelledby="headingOne" data-parent="#accordion">
                <Collapse in={!this.state.collapseMenu}>
                  <div>
                  <table className="table table-striped qwe">
                    <tbody>
                      {myCompletedTasks.map(this.taskCallback, this)}
                    </tbody>
                  </table>
                  </div>
                </Collapse>             
              </div>
            </div>
        </div>

        <Modal show={this.state.show || false}>
          <Modal.Header closeButton onClick={() => this.toogleModal(false)}>
            <Modal.Title>Edit</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form>
              <Form.Group className="mb-3" controlId="formBasicEmail">
                <Form.Control value={name} onChange={this.changeTaskNameForUpdate}
                 size="lg" type="text" placeholder="Meine Aufgabe ist..." />
                <Form.Text className="text-muted">
                  Geben Sie eine neue Aufgabe ein.
                </Form.Text>
              </Form.Group>
              <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                <Form.Control as="textarea" rows={3} value={note} onChange={this.changeNoteForUpdate} /><Form.Text className="text-muted">
                  Hier können Sie eine genauere Beschreibung hinzufügen.
                </Form.Text>
              </Form.Group>
              <Form.Group controlId="validationFormik03">
                <Form.Label>Date</Form.Label>                
                    <DatePicker
                      selected={dueDate}
                      dateFormat="dd.MM.yyyy"
                      onChange={this.changeDueDateForUpdate}
                      className="form-control"
                      minDate={today}
                    />
              </Form.Group>              
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="primary" type="submit" onClick={() => this.updateClick()}>
              Update
            </Button>          
          </Modal.Footer>
        </Modal>

      </div>
    )
  }
}