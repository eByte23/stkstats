import { GetStaticProps } from "next"
type TeamPageProps = {

}

const TeamPage = () => {

    return (
        <div className="flex flex-col justify-center items-center mx-auto ">
            <h1 className="text-3xl">Team Page</h1>
            <h1 className="text-2xl">Coming soon!</h1>
        </div>
    )
}



export const runtime = 'edge';
export default TeamPage



export const getStaticProps: GetStaticProps<TeamPageProps, { playerShortId?: string }> = async ({ params }) => {

    // const gameShortId = params?.["playerShortId"]
    // console.log("getStaticProps", { shortId: gameShortId })

    // const content = fs.readFileSync(process.cwd() + `/data/player-output/${gameShortId}.json`).toString()

    // const player = JSON.parse(content) as IndividualPlayer || []

    return {
        props: {
            // player: player || {},
        },
    }
}

export const getStaticPaths = async () => {
    // const content = fs.readFileSync(process.cwd() + "/data/players.json").toString()

    // const players = JSON.parse(content) as PlayerShortIds || []

    // const paths = players
    //     .map((playerShortId) => ({ params: { playerShortId: playerShortId } }))

    return {
        paths: [],
        fallback: true,
    }
}

