import React from 'react';

import InputForm from './components/inputForm.jsx';
import ResultView from './components/resultView.jsx';
import Instruction from './components/instruction.jsx';
import ErrorMessage from './components/errorMessage.jsx';

class App extends React.Component{

	state = {
		equation: '',
		tokens: false,
		firstLoad: true
	};
	
	onCalculate = (state)=>{
		this.setState({
			equation: state.equation,
			tokens: state.tokens,
			firstLoad: false,
			errorMessage: null
		});
	}

	onError = (error)=>{
		this.setState({errorMessage: error.message});
	}

	render(){
		let results = <Instruction/>
		if(!this.state.firstLoad) 
			results = <ResultView onError={this.onError} equation={this.state.equation} toknes={this.state.tokens}/>

		if(this.state.errorMessage)
			results = <ErrorMessage message={this.state.errorMessage}/>;
			
		return (
			<div>
				<header>
					<div className="s-container">
						<div className="s-row s-text--center">
							<h1>Kalkulatoruj</h1>
						</div>
						<InputForm onCalculate={this.onCalculate}/>
					</div>
				</header>
				<div className="s-container">
					{results}
				</div>
			</div>
		);
	}
}

export default App;