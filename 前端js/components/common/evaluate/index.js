import React, { PropTypes, Component } from 'react'

export default class Evaluate extends Component{
	render(){
		return(
			<div className="evaluate">
	            <button className="save"></button> <p>35</p>
	            <button className="thumbs_up"></button> <p>800</p>
	            <button className="chat"></button> <p>600</p>
	            <div className="clr"></div>
	        </div>		
		)
	}
}