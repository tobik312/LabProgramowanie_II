import React from 'react';

import Card from './card.jsx';
import TokensView from './api/tokensView.jsx';
import ValueView from './api/valueView.jsx';
import ChartView from './api/chartView.jsx';

class ResultView extends React.Component{

	onError = (error)=>{
		this.props.onError(error);
	}

    render(){
		return (
			<Card>
				<h1>Wynik</h1>
				{this.props.toknes===true ? <TokensView onError={this.onError} equation={this.props.equation}/> : null}
				<ValueView onError={this.onError} equation={this.props.equation}/>
				<ChartView onError={this.onError} equation={this.props.equation}/>
			</Card>
		);
    }
  }
  
export default ResultView;