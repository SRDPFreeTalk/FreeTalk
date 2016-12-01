import React, { PropTypes, Component } from 'react'
import Notice from '../noticeItem'

export default class NoticeList extends Component{
	render(){
		let Notices = [{
			Nid: "",
			username: "",
			userAvatar: "",
			isNew: "",
			time: "",
			content: "",
		}]

		return(
			<div className='notice-list'>
				{Notices.map(item=>
					<Notice content={item} />
				)}
			</div>
		)
	}
}