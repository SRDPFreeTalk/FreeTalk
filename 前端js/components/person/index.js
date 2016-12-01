import React, { PropTypes, Component } from 'react'
import PersonHeader from './personHeader/'
import PersonMain from './personMain/'

export default class Person extends Component{
	render(){
		let info = {
			id: "",
			username: "",
			userAvatar: "",
			concern: "",
			fans: "",
			introduce: "",
		}

		return (
			<div className="person">
				<PersonHeader info={info} />
				<PersonMain />
			</div>
		)
	}
}