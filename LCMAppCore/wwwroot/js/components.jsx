class LCMForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            algorithmTypeId: '',
            inputs: '',
            timeComplexity: '',
            spaceComplexity: '',
            result: '',
            inputsError: '',
            algorithmError:''
        };

        this.handleAlgorithmTypeIdChange = this.handleAlgorithmTypeIdChange.bind(this);
        this.handleInputsChange = this.handleInputsChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    refreshHistory() {
        var table = $("#histories").DataTable({
            destroy: true,
            ajax: {
                url: 'lcm/getHistories',
                dataSrc: ""
            },
            columns: [
                {
                    data: "inputs"

                },
                {
                    data: "algorithmType.name"

                },
                {
                    data: "timeComplexity"

                },
                {
                    data: "spaceComplexity"

                },
                {
                    data: "result"

                }

            ]
        });


    }

    handleInputsChange(e) {
        this.setState({ inputs: e.target.value });
    }

    handleAlgorithmTypeIdChange(e) {
        this.setState({ algorithmTypeId: e.target.value });
    }

    validate = () => {
        let inputsError = '';
        let algorithmError = '';
        let isValid = true;
        if (!this.state.inputs)
            inputsError = 'Inputs is/are required';
        
        if (!this.state.algorithmTypeId)
            algorithmError = 'Algorithm is required';

        if (inputsError) {
            this.setState({ inputsError: inputsError });
            isValid = false;
        }
        else
            this.setState({ inputsError: '' });

        if (algorithmError) {
            this.setState({ algorithmError: algorithmError });
            isValid = false;
        }
        else
            this.setState({ algorithmError: '' });

        return isValid;
    }

    handleSubmit(e) {
        e.preventDefault();
        const algorithmTypeId = this.state.algorithmTypeId.trim();
        const inputs = this.state.inputs.trim();

        const isValid = this.validate();

        if (!isValid)
            return;
    

        const data = new FormData();

        data.append('inputs', inputs);
        data.append('algorithmTypeId', algorithmTypeId);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ result: data.result, timeComplexity: data.timeComplexity, spaceComplexity: data.spaceComplexity });
        };
        xhr.send(data);

        //this.setState({ algorithmTypeId: '', inputs: '' });
    }

    render() {
        return (
            <form className="LCMForm form-horizontal" onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="email">Algorithm:</label>
                    <div className="col-sm-10">

                        <select className="form-control" onChange={this.handleAlgorithmTypeIdChange}>
                            <option value="">Select One</option>
                            <option value="1">Best time complexity</option>
                            <option value="2">Best space complexity</option>
                            <option value="3">Optimal time and space complexity</option>
                        </select>

                        <span style={{ color: 'maroon' }}>{this.state.algorithmError}</span>
                    </div>
                </div>
               

                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="email">Inputs:</label>
                    <div className="col-sm-10">

                        <textarea
                            type="text" className="form-control"
                            placeholder="Enter comma separated numbers"
                            value={this.state.inputs}
                            onChange={this.handleInputsChange}
                        />
                        <span style={{ color: 'maroon' }}>{this.state.inputsError}</span>
                    </div>
                </div>
               

                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="email">Time:</label>
                    <div className="col-sm-10" style={{ marginTop: '7px' }}>

                        <label>{this.state.timeComplexity}</label>
                    </div>
                </div>

                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="email">Space:</label>
                    <div className="col-sm-10" style={{ marginTop: '7px' }}>

                        <label>{this.state.spaceComplexity}</label>
                    </div>
                </div>

                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="email">Result:</label>
                    <div className="col-sm-10" style={{marginTop:'7px'}}>

                        <label>{this.state.result}</label>
                    </div>
                </div>

                <div className="form-group">
                    <div className="col-md-offset-2 col-md-10">
                        <input type="submit" value="Calculate LCM" className="btn btn-default" /> &nbsp;
                        <input type="button" onClick={this.refreshHistory}
                            className="btn btn-default" data-toggle="modal"
                            data-target="#addProducer" value="History" />
                    </div>
                </div>

               
            </form>
        );
    }
}

ReactDOM.render(
    <LCMForm submitUrl="/lcm/calculate" pollInterval={2000}/>,
    document.getElementById('content'),
);