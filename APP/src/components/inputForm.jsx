import React from 'react';

import Checkbox from './checkbox.jsx';

class InputForm extends React.Component{

	constructor(props){
		super(props);
		this.state = {
			equation: '',
			method: '',
			tokens: false
		};
		this.calculate = this.calculate.bind(this);
	}
	
	calculate = (e)=>{
		e.preventDefault();
		this.props.onCalculate(this.state);
	}

	setTokens = (check)=>{
		this.setState({tokens: check});
	}

	setEquation = (e)=>{
		this.setState({equation: e.target.value});
	}

    render(){
		return (
			<div className="input-form">
				<form className="s-row" onSubmit={this.calculate}>
					<div className="s-col s-small--12 s-medium--8 s-large--10">
						<input className="s-field--primary" placeholder="Wpisz równanie np.sin(3*x) {x:2}" type="text" onInput={this.setEquation}/>
					</div>
					<div className="s-col s-small--12 s-medium--4 s-large--2">
						<button type="summit" className="s-button--danger s-button--block">
							Oblicz
						</button>
					</div>
				</form>
				<div className="s-flex__row s-valigner">
					<div className="s-flex__col s-text--right">
						<Checkbox onCheck={this.setTokens}>Pokaż tokeny</Checkbox>
					</div>
				</div>
			</div>
		  );
	}	
}

export default InputForm;