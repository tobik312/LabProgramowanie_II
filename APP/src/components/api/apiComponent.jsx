import React from 'react';

class APIComponent extends React.Component{

    state = {
        load: false
    }

    static getVariables(equation){
		//Here you have an example why ES6 is awesome.
		if(/\{(.*?)\}$/g.test(equation))
			return Object.fromEntries(equation.match(/\{(.*?)\}$/g)[0].match(/(([a-z]+):(-?(\d*)(\.)?(\d+))),?/g)
						.map(variablePair => 
							variablePair.split(':').map((v,k) => k===1 ? parseFloat(v) : v)
						));
		else
			return {};
	}

	load(){}

	componentDidMount(prevProps,prevState){
        this.load();    
    }

    componentDidUpdate(prevProps,prevState){
		if(prevProps.equation!==this.props.equation){
			this.setState({load: false});
			this.load();
		}
    }

    loadData(type){
		let url = `http://${window.location.hostname}:5000/api/`;
		let variables = APIComponent.getVariables(this.props.equation);
		let xValue = (variables.x!==undefined) ? variables.x : 0;

		if(type==="value") 
			url += `calculate?x=${xValue}&`;
		else if(type==="tokens")
			url += `tokens?`;
		else if(type==='xy'){
			if(this.state.mounted)
				url += `calculate/xy?from=${this.state.from}&to=${this.state.to}&n=${this.state.n}&`;
		}

		url+=`formula=${encodeURIComponent(this.props.equation)}`;

		fetch(url).then(res => res.json())
        .then(
            (r) =>{
				if(r.status==='ok' && this.state.load>=0){
					let state = {
						load: true,
						x: xValue,
						reload: false
					};
					state[type] = r.result
					this.setState(state);
				}else
					this.loadFailed(r);
            },
            (e)=>{
                this.loadFailed({message: 'Nie można nawiązać połączenia z API!'});
            }
        );
	}

	loadFailed(error){
		this.setState({load: false});
		this.props.onError(error);
	}

}

export default APIComponent;
