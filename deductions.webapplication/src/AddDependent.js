import * as root from "react-dom";
import {Component} from "react";
import './AddDependent.css';
import logo from "./logo.svg";

// eslint-disable-next-line no-undef
class AddDependent extends Component {
    constructor(props) {
        super(props);
        this.state = { items: [], text: '' };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleGetDeduction = this.handleGetDeduction.bind(this);
    }

    render() {
        return (
            <div className="Dependents">
                <label>Employee Name:  </label>
                <input
                    /*className="Employee-input"
                      onChange={this.handleChange}
                      value={this.state.text}*/
                />
                <h4>Dependents to Add</h4>
                <form onSubmit={this.handleSubmit}>
                    <label>
                       Full Name:
                    </label>
                    <input
                        className="Add-dependent"
                        onChange={this.handleChange}
                        value={this.state.text}
                    />
                    <button>
                        Add Dependent
                    </button>
                    <DependentList items={this.state.items} />
                </form>
                <button id="Post-employee-info" onClick={this.handleGetDeduction}>Get Deduction Amount</button>
            </div>
        );
    }

    handleChange(e) {
        this.setState({ text: e.target.value });
    }

    handleSubmit(e) {
        e.preventDefault();
        if (this.state.text.length === 0) {
            return;
        }
        const newItem = {
            text: this.state.text,
            id: Date.now()
        };
        this.setState(state => ({
            items: state.items.concat(newItem),
            text: ''
        }));
    }

    handleGetDeduction = () => {

        const jsonData = {
            "EmployeeName": "John Doe",
            "Dependents": [
                {
                    "name": "Tom Willis"
                },
                {
                    "name": "Aaron Lewis"
                }
            ]
        };

        fetch('http://localhost:5001/api/v1/deductions', {

            method: 'POST',
            headers : {'Accept': 'application/json', 'Content-Type': 'application/json', 'Access-Control-Allow-Origin' : '*' },
            body: JSON.stringify(jsonData)

        })
        .then(response => response.json())
        .then(data => this.setState({ postId: data.id }))
    }
}

class DependentList extends Component {
    render() {
        return (
            <ul>
                {this.props.items.map(item => (
                    <li key={item.id}>{item.text}</li>
                ))}
            </ul>
        );
    }
}

export default AddDependent;