import React, { PropTypes, Component } from 'react'

export default class Header extends Component {

	render(){
		return (
			<header className="header">
		        <button className="showMenu"></button>
		        <form action="">
		            <input className="input" type="text" />
		            <a href=""><img src="./img/magnify.png" alt="search"></a>
		        </form>
		        <div class="clr"></div>
    		</header>
		);
	}
}