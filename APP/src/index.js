import React from 'react';
import ReactDOM from 'react-dom';

import * as serviceWorker from './serviceWorker';

import './styles/sculptor.min.css';
import './styles/app.css';

import App from './App.jsx';

ReactDOM.render(<App/>,document.getElementById('root'));

/*Dev stuff*/
serviceWorker.unregister();
